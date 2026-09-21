namespace MultiGameApp;

public class GameFactory
{
    public Game CreateGame(GameType gameType, GameMode gameMode, int boardSize)
    {
        if (!Enum.IsDefined(gameMode))
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
        Player playerTwo =
            mode == GameMode.HUMAN_VS_HUMAN
                ? new HumanPlayer(2, "Player 2", playerTwoPieces)
                : new ComputerPlayer(2, "Computer", playerTwoPieces);

        return [new HumanPlayer(1, "Player 1", playerOnePieces), playerTwo];
    }

    private void ValidateBoardSize(int boardSize, int requiredSize, string gameName)
    {
        if (boardSize != requiredSize)
            throw new ArgumentOutOfRangeException(
                nameof(boardSize),
                $"{gameName} requires a equal board size of {requiredSize}"
            );
    }
}
