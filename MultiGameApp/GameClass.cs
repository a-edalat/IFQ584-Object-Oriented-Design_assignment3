namespace MultiGameApp;

public abstract class Game
{
    // fields
    private readonly List<Board> _boards;
    private readonly List<Player> _players;
    private readonly MoveHistory _moveHistory;
    private Player _currentPlayer;

    // private string gameMode;
    // private bool isGameOver;
    // private string result;

    // constructor
    public Game(List<Player> players, List<Board> boards, GameMode mode)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(boards);

        if (players.Count == 0)
        {
            throw new ArgumentException("At least one valid player is required.", nameof(players));
        }

        if (boards.Count == 0)
        {
            throw new ArgumentException("At least one valid board is required.", nameof(boards));
        }

        foreach (Player player in players)
        {
            if (player is null)
            {
                throw new ArgumentException("A player cannot be null.", nameof(players));
            }
        }

        foreach (Board board in boards)
        {
            if (board is null)
            {
                throw new ArgumentException("A board cannot be null.", nameof(boards));
            }
        }

        if (!Enum.IsDefined(mode))
        {
            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        _players = players;
        _boards = boards;
        _moveHistory = new MoveHistory(new List<Move>(), -1);
        _currentPlayer = players[0];

        Mode = mode;
        Result = GameResult.IN_PROGRESS;
    }

    // getters
    public GameMode Mode { get; }
    public GameResult Result { get; protected set; }

    public bool IsGameOver
    {
        get { return Result != GameResult.IN_PROGRESS; }
    }

    // A caller can inspect these lists but cannot add or remove items through them.
    public IReadOnlyList<Board> Boards
    {
        get { return _boards.AsReadOnly(); }
    }

    public IReadOnlyList<Player> Players
    {
        get { return _players.AsReadOnly(); }
    }

    public IReadOnlyList<Move> Moves
    {
        get { return _moveHistory.GetMoves(); }
    }

    public int CurrentMoveIndex
    {
        get { return _moveHistory.GetCurrentIndex(); }
    }

    // Methods

    // in class I only check if the inputs are all numbers and within range
    // the check for if value is correctly picked form allowed pool and similar
    // are done within each game specifically
    // in my CRC this is abstract, but I ended up having a common IsValidMove
    // and will have specifics later
    // Made this protected as I am going to use makemove to call this to validate
    // with protected, derived classes can also use this
    protected virtual bool IsValidMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move);

        if (!ReferenceEquals(move.GetPlayer(), _currentPlayer))
        {
            return false;
        }

        if (!_currentPlayer.HasPiece(move.GetPiece()))
        {
            return false;
        }

        int boardIndex = move.GetBoardIndex();

        if (boardIndex < 0 || boardIndex >= _boards.Count)
        {
            return false;
        }

        Board selectedBoard = _boards[boardIndex];
        int row = move.GetRow();
        int column = move.GetColumn();

        if (!selectedBoard.IsWithinBoard(row, column))
        {
            return false;
        }

        return selectedBoard.IsCellEmpty(row, column);
    }


    // adding tmp makemove - can be removed later, boilerplate for me to use when testing game subclasses
    public bool MakeMove(Move move)
    {

        ArgumentNullException.ThrowIfNull(move);

        if (IsGameOver || !IsValidMove(move))
        {
            return false;
        }

        Player player = move.GetPlayer();
        Board selectedBoard = _boards[move.GetBoardIndex()];

        // Place first so a board error cannot consume the player's piece.
        selectedBoard.PlaceMove(move);

        if (!player.UsePiece(move.GetPiece()))
        {
            selectedBoard.RemoveMove(move);
            return false;
        }

        _moveHistory.AddMove(move);
        Result = EvaluateResult();

        if (Result == GameResult.IN_PROGRESS)
        {
            SwitchPlayer();
        }

        return true;
    }

    // public void ApplyMove(Move move)
    // {
    //     Board selectedBoard = _boards[move.GetBoardIndex()];

    //     selectedBoard.PlaceMove(move);
    // }

    public bool UndoMove()
    {
        if (_moveHistory.GetCurrentIndex() < 0)
        {
            return false;
        }

        // Inspect before changing the history index. If removal fails, history stays put.
        Move moveToUndo = _moveHistory.GetMoves()[_moveHistory.GetCurrentIndex()];
        Board selectedBoard = _boards[moveToUndo.GetBoardIndex()];
        Player player = moveToUndo.GetPlayer();

        selectedBoard.RemoveMove(moveToUndo);
        player.RestorePiece(moveToUndo.GetPiece());
        _moveHistory.UndoLastMove();

        _currentPlayer = player;
        Result = EvaluateResult();

        return true;
    }

    public bool RedoMove()
    {
        int nextMoveIndex = _moveHistory.GetCurrentIndex() + 1;
        List<Move> moves = _moveHistory.GetMoves();

        if (nextMoveIndex >= moves.Count)
        {
            return false;
        }

        Move moveToRedo = moves[nextMoveIndex];

        if (!IsValidMove(moveToRedo))
        {
            throw new InvalidOperationException("The saved move is no longer valid for redo.");
        }

        Board selectedBoard = _boards[moveToRedo.GetBoardIndex()];
        Player player = moveToRedo.GetPlayer();
        selectedBoard.PlaceMove(moveToRedo);

        if (!player.UsePiece(moveToRedo.GetPiece()))
        {
            selectedBoard.RemoveMove(moveToRedo);
            throw new InvalidOperationException("The piece required for redo is unavailable.");
        }

        _moveHistory.RedoLastMove();
        _currentPlayer = player;
        Result = EvaluateResult();

        if (Result == GameResult.IN_PROGRESS)
        {
            SwitchPlayer();
        }

        return true;
    }

    // made it protected so the concrete subclasses be able to use it easily
    protected void SwitchPlayer()
    {
        int currentPlayerIndex = _players.IndexOf(_currentPlayer);

        if (currentPlayerIndex < 0)
        {
            throw new InvalidOperationException("The current player is not in the players list.");
        }

        int nextPlayerIndex = currentPlayerIndex + 1;

        if (nextPlayerIndex >= _players.Count)
        {
            nextPlayerIndex = 0;
        }

        _currentPlayer = _players[nextPlayerIndex];
    }

    public Player GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public abstract List<Move> GetValidMoves();
    public abstract bool WouldMoveWin(Move move);
    public abstract GameResult EvaluateResult();
    public abstract string GetHelpText();

// commenting these two out and replace thme with a function in GameController
    // public void TakeTurn()
    // {
    //     Move proposedMove = _currentPlayer.GetMove();
    //     MakeMove(proposedMove);
    // }

    // public void StartGame()
    // {
    //     while (Result == GameResult.IN_PROGRESS)
    //     {
    //         TakeTurn();
    //     }
    // }
} // closes Game class
