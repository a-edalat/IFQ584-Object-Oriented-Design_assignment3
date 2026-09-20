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
        BoardSize = board.Rows;
        TargetSum = BoardSize * (BoardSize * BoardSize + 1) / 2;

        // true / false for even / odd
        ValidatePlayerNumbers(_players[0], true, "Player one");
        ValidatePlayerNumbers(_players[1], false, "Player one");
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
        //
    }

    private static List<Board> CreateBoardList(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (board.Rows < 3 || board.Rows != board.Columns)
            throw new ArgumentException(
                "Numerical Tic-Tac-Toe requires a square board, of at least 3x3."
            );

        return [board];
    }

    public override List<Move> GetValidMoves()
    {
        //
    }

    private List<NumberPiece> GetAvailableNumbers(Player player)
    {
        //
    }

    protected override bool IsValidMove(Move move)
    {
        //
    }

    private bool PlayerOwnsNumber(Player player, int value)
    {
        //
    }

    private bool IsNumberUnused(int value)
    {
        //
    }

    public override bool WouldMoveWin(Move move)
    {
        //
    }

    private bool HasWinningLine()
    {
        //
    }

    private bool CheckLineSum(int startingRow, int startingColumn, int rowChange, int columnChange)
    {
        //
    }

    protected override GameResult EvaluateResult()
    {
        //
    }

    private GameResult GetCurrentPlayerWinResult()
    {
        //
    }

    public override string GetHelpText()
    {
        return $"Place one of your unused numbers in an empty cell. Player one uses odd numbers. Player two uses even numbbers. Complete a row, column, or diagonal that sums to {TargetSum} to win.";
    }
}
