using System.Reflection.Metadata;

namespace MultiGameApp;

public class GomokuGame : Game
{
    // A2 spec says 15x15 or less for presentation
    public const int BoardSize = 15;
    public const int WinningLength = 5;

    private readonly Board _board;
    private readonly List<Player> _players;
    private GameResult _result;

    // each row contains one direction
    // opposite directions are checked as matched pairs
    private static readonly int[,] Directions =
    {
        // vertical
        { 1, 0 },
        // horizontal
        { 0, 1 },
        // descending diag
        { 1, 1 },
        // ascending diag
        { 1, -1 },
    };

    // assuming functions from CRC/Class diag until codified
    public GomokuGame(List<Player> players, Board board, gameMode mode)
        : base(ValidatePlayers(players), CreateBoardList(board), mode)
    {
        _players = players;
        _board = BoardSize;

        ValidatePlayerMarks();
    }

    public override List<Move> GetValidMoves()
    {
        //
    }

    protected override bool IsValidMove(Move move)
    {
        //
    }

    public override bool WouldMoveWin(Move move)
    {
        //
    }

    public override bool CheckGameOver()
    {
        //
    }

    public override string GetResult()
    {
        //
    }

    public override string GetHelpText()
    {
        //
    }

    private bool TryFindWinningSymbol(out string winningSymbol)
    {
        //
    }

    private bool HasWinningLine(int row, int column, string symbol)
    {
        //
    }

    private int CountDirection(
        int startingRow,
        int startingColumn,
        int rowChange,
        int columnChange,
        string symbol
    )
    {
        //
    }

    private GameResult GetWinResult(string winningSymbol)
    {
        //
    }

    private void ValidatePlayerMarks()
    {
        //
    }

    private static MarkPiece GetPlayerMark(Player player)
    {
        //
    }

    private static List<Player> ValidatePlayers(List<Player> players)
    {
        //
    }

    private static List<Board> CreateBoardList(Board board)
    {
        //
    }
}
