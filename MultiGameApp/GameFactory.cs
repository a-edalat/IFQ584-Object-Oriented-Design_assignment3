namespace MultiGameApp;

public class GameFactory
{
    public Game CreateGame(GameType gameType, GameMode gameMode, int boardSize)
    {
        if (!Enum.IsDefined(typeof(GameMode), gameMode))
            throw new ArgumentOutOfRangeException(nameof(gameMode), "The game mode is invalid.");

        return gameType switch
        {
            GameType.NUMERICAL_TIC_TAC_TOE => CreateNumericalTTTGame(gameMode, boardSize),
            GameType.NOTAKTO => CreateNotaktoGame(gameMode, boardSize),
            GameType.GOMOKU => CreateGomokuGame(gameMode, boardSize),

            _ => throw new ArgumentOutOfRangeException(nameof(gameType), "invalid game type."),
        };
    }

    private Game CreateNumericalTTTGame(GameMode mode, int boardSize)
    {
        throw new NotImplementedException();
    }

    private Game CreateNotaktoGame(GameMode mode, int boardSize)
    {
        throw new NotImplementedException();
    }

    private Game CreateGomokuGame(GameMode mode, int boardSize)
    {
        throw new NotImplementedException();
    }

    private List<Player> CreatePlayers(
        GameMode mode,
        List<Piece> playerOnePieces,
        List<Piece> playerTwoPieces
    )
    {
        throw new NotImplementedException();
    }

    private void ValidateBoardSize(
        int boardSize,
        int minimumSize,
        int? requiredSize,
        string gameName
    )
    {
        throw new NotImplementedException();
    }
}
