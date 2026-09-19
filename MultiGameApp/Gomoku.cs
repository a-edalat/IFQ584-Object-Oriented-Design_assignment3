using System.Linq.Expressions;
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
    public GomokuGame(List<Player> players, Board board, gameMode mode)
        : base(ValidatePlayers(players), CreateBoardList(board), mode)
    {
        _players = players;
        _board = BoardSize;

        ValidatePlayerMarks();
    }

    public override List<Move> GetValidMoves()
    {
        // get player and mark
        // visit every board position
        // if empty cell
        //  add move
        // return collected moves
    }

    protected override bool IsValidMove(Move move)
    {
        // check
        // index, boundaries, empty cell
        // move belongs to player
        // markpiece with players symbol
        // true if yes
    }

    public override bool WouldMoveWin(Move move)
    {
        // count the mark as centre of possible line
        // count matching neighbours forward
        // count matching neighbours opposite
        // true if combined <=5
        // false otherwise and remove
    }

    protected override GameResult EvaluateResult()
    {
        // for each row and col
        // if cell has piece
        // if winning line is true return get winresult (specifies which player won)
    }

    public override string GetHelpText()
    {
        // return how to play gomoku
    }

    private bool HasWinningLine(int row, int column, string symbol)
    {
        // visit each occupied board position
        // if winning line found
        // compare symbol with player and return correct - WIN
        // if no line wins and board full - DRAW
        // other - IN PROGRESS
    }

    private int CountDirection(
        int startingRow,
        int startingColumn,
        int rowChange,
        int columnChange,
        string symbol
    )
    {
        // start at the neighbouring cell
        // continue while the position is on the board
        // stop when the cell does not contain the required mark
        // otherwise increase the count and move one more cell in that direction
        // return the uninterrupted mark count
    }

    private GameResult GetWinResult(string winningSymbol)
    {
        // return symbol owned by player as winner
        // else throw invalid error
    }

    private void ValidatePlayerMarks()
    {
        // gomoku players must use different marks
    }

    private static MarkPiece GetPlayerMark(Player player)
    {
        // return player mark
        //  return error if none
    }

    private static List<Player> ValidatePlayers(List<Player> players)
    {
        // return error if player count != 2
    }

    private static List<Board> CreateBoardList(Board board)
    {
        // if board null or not 15x15 return error
        // return board
    }
}
