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
        // currentPlayer initialisation
        // checking validity of the players before selecting initial player
        // I decided not to check the number of players here to allow future extenstion to the game,
        // if a future game has more then 2 players, or it is a signle player game, then this class is
        // configurable enough, each game class can check if number of players matches its requirement
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }
        if (players.Count == 0)
        {
            throw new ArgumentException(
                "You currently have zero players, at least one player is required.",
                nameof(players)
            );
        }

        // boards initialisation
        // check if each game has at least a board and the boards is not null
        // Each game then can check if the number of boards matches their requirement
        if (boards == null)
        {
            throw new ArgumentNullException(nameof(boards));
        }
        if (boards.Count == 0)
        {
            throw new ArgumentException(
                "You currently have zero boards, at least one board is required.",
                nameof(boards)
            );
        }

        // gameMode
        if (Enum.IsDefined(typeof(GameMode), mode) == false)
        {
            throw new ArgumentOutOfRangeException(nameof(mode), "The game mode is not valid.");
        }

        _players = players;
        _boards = boards;
        _moveHistory = new MoveHistory();
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
        ArgumentNullException.ThrowIfNull(move); //Making sure move is not null

        // assuming move has board #, row, col and value in that order

        int row = move.Row;
        int col = move.Column;
        int boardIndex = move.BoardIndex;

        if (boardIndex < 0 || boardIndex >= _boards.Count)
        {
            return false;
        }
        // currently using .Rows and .Columns, but during implementation I may
        // Change this to size. so an i and j values essentially.
        // Now that I know boardindex is valid, I can read the board
        Board selectedBoard = _boards[boardIndex];

        // if (row < 0 || row >= selectedBoard.Rows)
        if (selectedBoard.IsWithinBoard(row, col) == false)
        {
            return false;
        }
        if (selectedBoard.IsCellEmpty(row, col) == false)
        {
            return false;
        }
        return true; // returning true after validation checks above
    }

    // adding tmp makemove - can be removed later, boilerplate for me to use when testing game subclasses
    public bool MakeMove(Move move)
    {
        if (IsGameOver)
        {
            return false;
        }

        if (IsValidMove(move) == false)
        {
            return false;
        }

        Player player = move.GetPlayer();

        if (!player.UsePiece(move.Piece))
        {
            return false;
        }

        ApplyMove(move);
        _moveHistory.AddMove(move);
        Result = EvaluateResult();

        if (Result == GameResult.IN_PROGRESS)
        {
            SwitchPlayer();
        }

        return true;
    }

    public void ApplyMove(Move move)
    {
        Board selectedBoard = _boards[move.BoardIndex];

        selectedBoard.PlaceMove(move);
    }

    public bool UndoMove()
    {
        if (_moveHistory.GetCurrentIndex() < 0)
        {
            return false;
        }

        Move moveToUndo = _moveHistory.UndoLastMove();
        Board selectedBoard = _boards[moveToUndo.BoardIndex];
        Player player = moveToUndo.GetPlayer();

        selectedBoard.RemoveMove(moveToUndo);
        player.RestorePiece(moveToUndo.Piece);

        _currentPlayer = player;
        Result = EvaluateResult();

        return true;
    }

    public bool RedoMove()
    {
        if (_moveHistory.GetCurrentIndex() >= _moveHistory.GetMoves().Count - 1)
        {
            return false;
        }

        Move moveToRedo = _moveHistory.RedoLastMove();
        Board selectedBoard = _boards[moveToRedo.BoardIndex];
        Player player = moveToRedo.GetPlayer();

        if (!player.UsePiece(moveToRedo.Piece))
        {
            throw new InvalidOperationException("The piece required for redo is not available.");
        }

        selectedBoard.PlaceMove(moveToRedo);
        _currentPlayer = moveToRedo.GetPlayer();
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

    public void TakeTurn()
    {
        Move proposedMove = _currentPlayer.GetMove();
        MakeMove(proposedMove);
    }

    public void StartGame()
    {
        while (Result == GameResult.IN_PROGRESS)
        {
            TakeTurn();
        }
    }
} // closes Game class
