namespace MultiGameApp;

public class NotaktoGame : Game
{
    public const int RequiredBoards = 3;
    public const int BoardSize = 3;

    private readonly List<Board> _boards;
    private readonly List<Player> _players;
    private readonly List<int> _inactiveBoards = [];
    private readonly string _symbol;

    public IReadOnlyList<int> InactiveBoards => _inactiveBoards;

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
            if (
                !board.IsWithinBoard(BoardSize - 1, BoardSize - 1)
                || board.IsWithinBoard(BoardSize, 0)
                || board.IsWithinBoard(0, BoardSize)
            )
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
        var validMoves = new List<Move>();
        var player = GetCurrentPlayer();
        var mark = GetPlayerMark(player);

        for (int i = 0; i < _boards.Count; i++)
        {
            var board = _boards[i];

            if (CheckBoardHasLine(board))
                continue;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board.IsCellEmpty(row, col))
                        validMoves.Add(new Move(player, i, row, col, mark));
                }
            }
        }

        return validMoves;
    }

    protected override bool IsValidMove(Move move)
    {
        if (!base.IsValidMove(move) || CheckBoardHasLine(_boards[move.BoardIndex]))
            return false;

        var player = GetCurrentPlayer();

        if (!ReferenceEquals(move.GetPlayer(), player))
            return false;

        return ReferenceEquals(move.Piece, GetPlayerMark(player))
            && move.Piece is MarkPiece mark
            && mark.Symbol == _symbol;
    }

    private bool CheckBoardHasLine(Board board)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            // check row or col
            if (CheckLine(board, i, 0, 0, 1) || CheckLine(board, 0, i, 1, 0))
                return true;
        }

        // check diags
        return CheckLine(board, 0, 0, 1, 1) || CheckLine(board, 0, BoardSize - 1, 1, -1);
    }

    private bool CheckLine(
        Board board,
        int startingRow,
        int startingColumn,
        int rowChange,
        int columnChange
    )
    {
        for (int i = 0; i < BoardSize; i++)
        {
            int row = startingRow + i * rowChange;
            int col = startingColumn + i * columnChange;

            if (board.GetCell(row, col) is not MarkPiece mark || mark.Symbol != _symbol)
                return false;
        }

        return true;
    }

    private void RefreshInactiveBoards()
    {
        _inactiveBoards.Clear();

        for (int i = 0; i < _boards.Count; i++)
        {
            if (CheckBoardHasLine(_boards[i]))
                _inactiveBoards.Add(i);
        }
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
