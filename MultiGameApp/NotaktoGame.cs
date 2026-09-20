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
        ArgumentNullException.ThrowIfNull(players);

        if (players.Count != 2)
            throw new ArgumentException("Notakto requires two players");

        return players;
    }

    private static List<Board> ValidateBoards(List<Board> boards)
    {
        ArgumentNullException.ThrowIfNull(boards);

        if (boards.Count != RequiredBoards)
            throw new ArgumentException("Notakto requires three boards");

        foreach (var board in boards)
        {
            if (board.Rows != BoardSize || board.Columns != BoardSize)
                throw new ArgumentException("Each Notakto board must be 3 x 3");
        }

        return boards;
    }

    private static MarkPiece GetPlayerMark(Player player)
    {
        foreach (var piece in player.AvailablePieces)
        {
            if (piece is MarkPiece mark)
                return mark;
        }

        throw new InvalidOperationException("Each Notakto player must have a mark");
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
        if (!IsValidMove(move))
            return false;

        // move that ends game makes the player lose
        return false;
    }

    public override GameResult EvaluateResult()
    {
        RefreshInactiveBoards();

        if (_inactiveBoards.Count < RequiredBoards)
            return GameResult.IN_PROGRESS;

        var losingPlayer = GetCurrentPlayer();

        if (ReferenceEquals(losingPlayer, _players[0]))
            return GameResult.PLAYER_TWO_WIN;
        if (ReferenceEquals(losingPlayer, _players[1]))
            return GameResult.PLAYER_ONE_WIN;

        throw new InvalidOperationException("The current player is not part of the game.");
    }

    public override string GetHelpText()
    {
        return "Players take turns placing the same marks on three 3 by 3 boards. A board becomes inactive when it contains a complete row, column, or diagonal. The player who completes a line on the final active board loses.";
    }
}
