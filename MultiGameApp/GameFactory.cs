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

    private static NumericalTTTGame CreateNumericalTTTGame(GameMode mode, int boardSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(boardSize, 3);

        var playerOnePieces = new List<Piece>();
        var playerTwoPieces = new List<Piece>();

        for (int val = 1; val <= boardSize * boardSize; val++)
        {
            var pieces = val % 2 != 0 ? playerOnePieces : playerTwoPieces;
            pieces.Add(new NumberPiece(val));
        }

        var players = CreatePlayers(mode, playerOnePieces, playerTwoPieces);
        return new NumericalTTTGame(players, new Board(boardSize, boardSize), mode);
    }

    private static NotaktoGame CreateNotaktoGame(GameMode mode, int boardSize)
    {
        ValidateBoardSize(boardSize, 3, "Notakto");

        var players = CreatePlayers(mode, [new MarkPiece("X")], [new MarkPiece("X")]);
        var boards = new List<Board>();

        for (int idx = 0; idx < NotaktoGame.RequiredBoards; idx++)
        {
            boards.Add(new Board(NotaktoGame.BoardSize, NotaktoGame.BoardSize));
        }

        return new NotaktoGame(players, boards, mode);
    }

    private static GomokuGame CreateGomokuGame(GameMode mode, int boardSize)
    {
        ValidateBoardSize(boardSize, GomokuGame.BoardSize, "Gomoku");

        var players = CreatePlayers(mode, [new MarkPiece("X")], [new MarkPiece("O")]);
        var board = new Board(GomokuGame.BoardSize, GomokuGame.BoardSize);

        return new GomokuGame(players, board, mode);
    }

    private static List<Player> CreatePlayers(
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

    private static void ValidateBoardSize(int boardSize, int requiredSize, string gameName)
    {
        if (boardSize != requiredSize)
            throw new ArgumentOutOfRangeException(
                nameof(boardSize),
                $"{gameName} requires a equal board size of {requiredSize}"
            );
    }
}
