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

    public Move ChooseMove(Game game, Player player)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(player);

        if (!ReferenceEquals(game.GetCurrentPlayer(), player))
            throw new ArgumentException("Supplied name of player is not the current player");

        var validMoves = game.GetValidMoves();

        if (validMoves.Count == 0)
            throw new InvalidOperationException("No valid moves available");

        foreach (var move in validMoves)
        {
            if (game.WouldMoveWin(move))
                return move;
        }

        return validMoves[_random.Next(validMoves.Count)];
    }
}
