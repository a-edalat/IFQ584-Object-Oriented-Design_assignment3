namespace MultiGameApp;

public class NumericalTTTGame : Game
{
    private readonly Board _board;

    private readonly List<Player> _players;

    public int BoardSize { get; }

    public int TargetSum { get; }

    public NumericalTTTGame (List<Player> players, Board board, GameMode mode) : base (ValidatePlayers(players), CreateBoardList(board), mode)
    {
        _players = players;
        _board = board;
        BoardSize = board.Rows;
        TargetSum = BoardSize * (BoardSize * BoardSize + 1) / 2;

        // true / false for even / odd
        ValidatePlayerNumbers(_players[0], true, "Player one");
        ValidatePlayerNumbers(_players[1], false, "Player one");

    }
}