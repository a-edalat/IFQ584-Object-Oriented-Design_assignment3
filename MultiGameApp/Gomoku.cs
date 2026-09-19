namespace MultiGameApp;

public class GomokuGame : Game
{
    public const int BoardSize = 15;
    public const int WinningLength = 5;

    private readonly Board _board;
    private readonly List<Player> _players;
    private GameResult _result;
    
}
