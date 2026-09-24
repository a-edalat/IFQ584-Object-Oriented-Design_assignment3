namespace MultiGameApp;

public class NumericalTTTGame : Game
{
    private readonly Board _board;

    private readonly List<Player> _players;

    public int BoardSize { get; }

    public int TargetSum { get; }

    public NumericalTTTGame(List<Player> players, Board board, GameMode mode)
        : base(ValidatePlayers(players), CreateBoardList(board), mode)
    {
        _players = players;
        _board = board;
        BoardSize = GetBoardSize(board);
        TargetSum = BoardSize * (BoardSize * BoardSize + 1) / 2;

        // true / false for odd / even
        ValidatePlayerNumbers(_players[0], true, "Player one");
        ValidatePlayerNumbers(_players[1], false, "Player two");
    }

    private static List<Player> ValidatePlayers(List<Player> players)
    {
        ArgumentNullException.ThrowIfNull(players);

        if (players.Count != 2)
            throw new ArgumentException("Numerical Tic-Tac-Toe requires two players.");

        return players;
    }

    private void ValidatePlayerNumbers(Player player, bool shouldBeOdd, string playerName)
    {
        // extract and sort numerical values assigned to player
        var numbers = player
            .AvailablePieces.OfType<NumberPiece>()
            .Select(number => number.Value)
            .OrderBy(value => value)
            .ToList();

        // build the complete set of odd or even numbers
        var expectedNumbers = Enumerable
            .Range(1, BoardSize * BoardSize)
            .Where(value => (value % 2 != 0) == shouldBeOdd)
            .ToList();

        // catch and throw errors
        if (
            numbers.Count != player.AvailablePieces.Count
            || !numbers.SequenceEqual(expectedNumbers)
        )
        {
            string numberType = shouldBeOdd ? "odd" : "even";

            throw new ArgumentException(
                $"{playerName} must have each {numberType} number from 1 to {BoardSize * BoardSize}"
            );
        }
    }

    private static List<Board> CreateBoardList(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        GetBoardSize(board);
        return [board];
    }

    private static int GetBoardSize(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        int size = 0;
        while (board.IsWithinBoard(size, 0))
            size++;

        if (size < 3 || !board.IsWithinBoard(0, size - 1) || board.IsWithinBoard(0, size))
            throw new ArgumentException(
                "Numerical Tic-Tac-Toe requires a square board of at least 3x3"
            );

        return size;
    }

    public override List<Move> GetValidMoves()
    {
        var moves = new List<Move>();
        var player = GetCurrentPlayer();
        var numbers = GetAvailableNumbers(player);

        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (!_board.IsCellEmpty(row, col))
                    continue;

                foreach (var number in numbers)
                    moves.Add(new Move(player, 0, row, col, number));
            }
        }

        return moves;
    }

    private List<NumberPiece> GetAvailableNumbers(Player player)
    {
        var numbers = new List<NumberPiece>();

        foreach (var piece in player.AvailablePieces)
        {
            if (piece is NumberPiece number && IsNumberUnused(number.Value))
                numbers.Add(number);
        }
        return numbers;
    }

    protected override bool IsValidMove(Move move)
    {
        if (!base.IsValidMove(move) || move.BoardIndex != 0)
            return false;

        var player = GetCurrentPlayer();
        if (!ReferenceEquals(move.GetPlayer(), player) || move.Piece is not NumberPiece number)
            return false;

        return PlayerOwnsNumber(player, number.Value) && IsNumberUnused(number.Value);
    }

    private static bool PlayerOwnsNumber(Player player, int value)
    {
        foreach (var piece in player.AvailablePieces)
        {
            if (piece is NumberPiece number && number.Value == value)
                return true;
        }

        return false;
    }

    private bool IsNumberUnused(int value)
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (_board.GetCell(row, col) is NumberPiece number && number.Value == value)
                    return false;
            }
        }

        return true;
    }

    public override bool WouldMoveWin(Move move)
    {
        if (!IsValidMove(move))
            return false;

        _board.PlaceMove(move);
        bool winningMove = HasWinningLine();
        _board.RemoveMove(move);

        return winningMove;
    }

    private bool HasWinningLine()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            // if (row || col) wins
            if (CheckLineSum(i, 0, 0, 1) || CheckLineSum(0, i, 1, 0))
                return true;
        }
        // if (diag 1 || diag 2) wins
        return CheckLineSum(0, 0, 1, 1) || CheckLineSum(0, BoardSize - 1, 1, -1);
    }

    private bool CheckLineSum(int startingRow, int startingColumn, int rowChange, int columnChange)
    {
        int sum = 0;

        for (int i = 0; i < BoardSize; i++)
        {
            int row = startingRow + i * rowChange;
            int col = startingColumn + i * columnChange;

            if (_board.GetCell(row, col) is not NumberPiece number)
                return false;

            sum += number.Value;
        }

        return sum == TargetSum;
    }

    public override GameResult EvaluateResult()
    {
        if (HasWinningLine())
            return GetCurrentPlayerWinResult();

        return _board.IsFull() ? GameResult.DRAW : GameResult.IN_PROGRESS;
    }

    private GameResult GetCurrentPlayerWinResult()
    {
        var player = GetCurrentPlayer();

        if (ReferenceEquals(player, _players[0]))
            return GameResult.PLAYER_ONE_WIN;

        if (ReferenceEquals(player, _players[1]))
            return GameResult.PLAYER_TWO_WIN;

        throw new InvalidOperationException("The current player is not part of this game.");
    }

    public override string GetHelpText()
    {
        return $"Place one of your unused numbers in an empty cell. Player one uses odd numbers. Player two uses even numbers. Complete a row, column, or diagonal that sums to {TargetSum} to win.";
    }
}
