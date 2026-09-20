using System.Reflection.Metadata;

namespace MultiGameApp;

public class NotaktoGame : Game
{
    public const int RequiredBoards = 3;
    public const int BoardSize = 3;

    private readonly List<Board> _boards;
    private readonly List<Player> _players;
    private readonly List<int> _inactiveBoards = [];

    public IReadOnlyList<int> InactiveBoard => _inactiveBoards;

    public NotaktoGame(List<Player> players, List<Board> boards, GameMode mode)
        : base(ValidatePlayers(players), ValidateBoards(boards), mode)
    {
        _players = players;
        _boards = boards;

        var playerOneMark = GetPlayerMark(players[0]);
        var playerTwoMark = GetPlayerMark(players[1]);

        if (playerOneMark.Symbol != playerTwoMark.Symbol)
            throw new ArgumentException("Notakto players must use the same mark.");

        _symbol = playerOneMark.Symbol;
        RefreshInactiveBoards();
    }

    private static List<Player> ValidatePlayers(List<Player> players)
    {
        //
    }

    private static List<Board> ValidateBoards(List<Board> boards)
    {
        //
    }

    private static MarkPiece GetPlayerMark(Player player)
    {
        //
    }

    public override List<Move> GetValidMoves()
    {
        //
    }

    protected override bool IsValidMove(Move move)
    {
        //
    }

    private bool CheckBoardHasLine(Board board)
    {
        //
    }

    private bool CheckLine(
        BlobReader board,
        int startingRow,
        int startingColumn,
        int rowChange,
        int columnChange
    )
    {
        //
    }

    private void RefreshInactiveBoards()
    {
        //
    }

    public override bool WouldMoveWin(Move move)
    {
        //
    }

    public override GameResult EvaluateResult()
    {
        //
    }

    public override string GetHelpText()
    {
        //
    }
}
