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
        //
    }

    private void ValidatePlayerNumbers(Player player, bool shouldBeOdd, string playerName)
    {
        //
    }

    private static List<Board> CreateBoardList(Board board)
    {
        //
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
        //
    }
}
