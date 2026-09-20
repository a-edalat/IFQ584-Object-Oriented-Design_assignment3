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
}
