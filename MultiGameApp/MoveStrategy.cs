namespace MultiGameApp;

public interface MoveStrategy
{
    Move ChooseMove(Game game, Player player);
}

public class WinningThenRandomStrategy : MoveStrategy
{
    private readonly Random _random;

    public WinningThenRandomStrategy(Random? random = null)
    {
        _random = random ?? Random.Shared;
    }

    public MoveStrategy ChooseMove(Game game, Player player)
    {
        // 
    }
}
