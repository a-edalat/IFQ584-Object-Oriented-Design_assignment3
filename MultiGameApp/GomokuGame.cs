namespace MultiGameApp;

public class GomokuGame : Game
{
    // A2 spec says 15x15; optional smaller for UI if needed.
    public const int BoardSize = 15;
    public const int WinningLength = 5;

    private readonly Board _board;
    private readonly List<Player> _players;

    // each tuple stores one direction for winning validation checks
    private static readonly (int Row, int Column)[] Directions =
    [
        // vertical
        (1, 0),
        // horizontal
        (0, 1),
        // descending diag
        (1, 1),
        // ascending diag
        (1, -1),
    ];

    // assuming functions from CRC/Class diag until codified
    // initialisation
    public GomokuGame(List<Player> players, Board board, GameMode mode)
        : base(ValidatePlayers(players), CreateBoardList(board), mode)
    {
        _players = players;
        _board = board;

        ValidatePlayerMarks();
    }

    private static List<Player> ValidatePlayers(List<Player> players)
    {
        ArgumentNullException.ThrowIfNull(players);

        if (players.Count != 2)
            throw new ArgumentException("Gomoku requires exactly two players.", nameof(players));

        return players;
    }

    private void ValidatePlayerMarks()
    {
        if (GetPlayerMark(_players[0]).Symbol == GetPlayerMark(_players[1]).Symbol)
            throw new ArgumentException("Gomoku players must use different marks.");
    }

    private static MarkPiece GetPlayerMark(Player player)
    {
        foreach (var piece in player.AvailablePieces)
        {
            if (piece is MarkPiece mark)
                return mark;
        }

        throw new InvalidOperationException("Each Gomoku player must have a mark.");
    }

    private static List<Board> CreateBoardList(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (!board.HasSize(BoardSize, BoardSize))
            throw new ArgumentException("Gomoku requires a 15 by 15 board.", nameof(board));

        return [board];
    }

    // moves
    public override List<Move> GetValidMoves()
    {
        var validMoves = new List<Move>();
        var player = GetCurrentPlayer();
        var mark = GetPlayerMark(player);

        for (int row = 0; row < BoardSize; row++)
        {
            for (int column = 0; column < BoardSize; column++)
            {
                if (_board.IsCellEmpty(row, column))
                    validMoves.Add(new Move(player, 0, row, column, mark));
            }
        }

        return validMoves;
    }

    protected override bool IsValidMove(Move move)
    {
        if (!base.IsValidMove(move) || move.GetBoardIndex() != 0)
            return false;

        var player = GetCurrentPlayer();
        if (!ReferenceEquals(move.GetPlayer(), player))
            return false;

        return move.GetPiece() is MarkPiece mark && mark.Symbol == GetPlayerMark(player).Symbol;
    }

    // win state
    public override bool WouldMoveWin(Move move)
    {
        if (!IsValidMove(move) || move.GetPiece() is not MarkPiece mark)
            return false;

        return HasWinningLine(move.GetRow(), move.GetColumn(), mark.Symbol);
    }

    private bool HasWinningLine(int row, int column, string symbol)
    {
        foreach (var direction in Directions)
        {
            int lineLength = 1;
            lineLength += CountDirection(row, column, direction.Row, direction.Column, symbol);
            lineLength += CountDirection(row, column, -direction.Row, -direction.Column, symbol);

            if (lineLength >= WinningLength)
                return true;
        }

        return false;
    }

    private int CountDirection(
        int startingRow,
        int startingColumn,
        int rowChange,
        int columnChange,
        string symbol
    )
    {
        int count = 0;
        int row = startingRow + rowChange;
        int column = startingColumn + columnChange;

        while (_board.IsWithinBoard(row, column))
        {
            if (_board.GetCell(row, column) is not MarkPiece mark || mark.Symbol != symbol)
                break;

            count++;
            row += rowChange;
            column += columnChange;
        }

        return count;
    }

    // game progress
    public override GameResult EvaluateResult()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int column = 0; column < BoardSize; column++)
            {
                if (
                    _board.GetCell(row, column) is MarkPiece mark
                    && HasWinningLine(row, column, mark.Symbol)
                )
                {
                    return GetWinResult(mark.Symbol);
                }
            }
        }

        return _board.IsFull() ? GameResult.DRAW : GameResult.IN_PROGRESS;
    }

    private GameResult GetWinResult(string winningSymbol)
    {
        if (GetPlayerMark(_players[0]).Symbol == winningSymbol)
            return GameResult.PLAYER_ONE_WIN;

        if (GetPlayerMark(_players[1]).Symbol == winningSymbol)
            return GameResult.PLAYER_TWO_WIN;

        throw new InvalidOperationException("The winning mark does not belong to a player.");
    }

    // help
    public override string GetHelpText()
    {
        return "Place your mark in an empty cell on the 15 by 15 board. "
            + "The first player to form an uninterrupted horizontal, vertical, "
            + "or diagonal line of five or more marks wins.";
    }
}
