using System.Text.Json;

namespace MultiGameApp;

public class SaveManager
{
    // fields
    private readonly string filePath;
    private readonly GameFactory gameFactory;

    // constructor
    public SaveManager(string filePath, GameFactory gameFactory)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("A file path is required.", nameof(filePath));
        }

        ArgumentNullException.ThrowIfNull(gameFactory);

        this.filePath = filePath;
        this.gameFactory = gameFactory;
    }

    // Methods
    public void SaveGame(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);

        GameType gameType;

        if (game is NumericalTTTGame)
        {
            gameType = GameType.NUMERICAL_TIC_TAC_TOE;
        }
        else if (game is NotaktoGame)
        {
            gameType = GameType.NOTAKTO;
        }
        else if (game is GomokuGame)
        {
            gameType = GameType.GOMOKU;
        }
        else
        {
            throw new ArgumentException("This game type cannot be saved.", nameof(game));
        }

        List<object> savedPlayers = new List<object>();

        foreach (Player player in game.Players)
        {
            savedPlayers.Add(
                new
                {
                    Id = player.Id,
                    IsComputer = player.IsComputer(),
                }
            );
        }

        List<object> savedMoves = new List<object>();

        foreach (Move move in game.Moves)
        {
            savedMoves.Add(
                new
                {
                    PlayerId = move.GetPlayer().Id,
                    BoardIndex = move.GetBoardIndex(),
                    Row = move.GetRow(),
                    Column = move.GetColumn(),
                    PieceValue = move.GetPiece().DisplayValue(),
                }
            );
        }

        var savedGame = new
        {
            Version = 1,
            GameType = gameType,
            Mode = game.Mode,
            BoardSize = game.Boards[0].GetDimensions().Rows,
            Players = savedPlayers,
            Moves = savedMoves,
            CurrentMoveIndex = game.CurrentMoveIndex,
            CurrentPlayerId = game.GetCurrentPlayer().Id,
            Result = game.Result,
        };

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string json = JsonSerializer.Serialize(savedGame, options);
        File.WriteAllText(filePath, json);
    }

    public Game LoadGame()
    {
        string json = File.ReadAllText(filePath);
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement savedGame = document.RootElement;

        if (savedGame.ValueKind != JsonValueKind.Object || ReadInt(savedGame, "Version") != 1)
        {
            throw new InvalidDataException("The save file version is not supported.");
        }

        GameType gameType = (GameType)ReadInt(savedGame, "GameType");
        GameMode mode = (GameMode)ReadInt(savedGame, "Mode");
        int boardSize = ReadInt(savedGame, "BoardSize");
        int savedMoveIndex = ReadInt(savedGame, "CurrentMoveIndex");
        int savedCurrentPlayerId = ReadInt(savedGame, "CurrentPlayerId");
        GameResult savedResult = (GameResult)ReadInt(savedGame, "Result");

        if (!Enum.IsDefined(gameType) || !Enum.IsDefined(mode) || !Enum.IsDefined(savedResult))
        {
            throw new InvalidDataException(
                "The save contains an invalid game type, mode, or result."
            );
        }

        if (
            !savedGame.TryGetProperty("Players", out JsonElement savedPlayers)
            || savedPlayers.ValueKind != JsonValueKind.Array
            || savedPlayers.GetArrayLength() != 2
        )
        {
            throw new InvalidDataException("The save must contain two players.");
        }

        if (
            !savedGame.TryGetProperty("Moves", out JsonElement savedMoves)
            || savedMoves.ValueKind != JsonValueKind.Array
            || savedMoveIndex < -1
            || savedMoveIndex >= savedMoves.GetArrayLength()
        )
        {
            throw new InvalidDataException("The saved move history is invalid.");
        }

        JsonElement playerOne = savedPlayers[0];
        JsonElement playerTwo = savedPlayers[1];

        if (
            playerOne.ValueKind != JsonValueKind.Object
            || playerTwo.ValueKind != JsonValueKind.Object
            || ReadInt(playerOne, "Id") != 1
            || ReadInt(playerTwo, "Id") != 2
            || ReadBool(playerOne, "IsComputer")
            || ReadBool(playerTwo, "IsComputer") != (mode == GameMode.HUMAN_VS_COMPUTER)
        )
        {
            throw new InvalidDataException("The saved player information is invalid.");
        }

        Game game;

        try
        {
            game = gameFactory.CreateGame(gameType, mode, boardSize);
        }
        catch (ArgumentException error)
        {
            throw new InvalidDataException("The saved game configuration is invalid.", error);
        }

        foreach (JsonElement savedMove in savedMoves.EnumerateArray())
        {
            if (savedMove.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidDataException("The save contains an invalid move.");
            }

            Move? move = FindReplayMove(game, savedMove);

            if (move is null || !game.MakeMove(move))
            {
                throw new InvalidDataException("The saved moves cannot be replayed.");
            }
        }

        while (game.CurrentMoveIndex > savedMoveIndex)
        {
            if (!game.UndoMove())
            {
                throw new InvalidDataException("The saved history index cannot be restored.");
            }
        }

        if (game.GetCurrentPlayer().Id != savedCurrentPlayerId || game.Result != savedResult)
        {
            throw new InvalidDataException("The saved state does not match its move history.");
        }

        return game;
    }

    public bool FileExists()
    {
        return File.Exists(filePath);
    }

    private static Move? FindReplayMove(Game game, JsonElement savedMove)
    {
        int playerId = ReadInt(savedMove, "PlayerId");
        int boardIndex = ReadInt(savedMove, "BoardIndex");
        int row = ReadInt(savedMove, "Row");
        int column = ReadInt(savedMove, "Column");
        string pieceValue = ReadString(savedMove, "PieceValue");

        foreach (Move move in game.GetValidMoves())
        {
            if (
                move.GetPlayer().Id == playerId
                && move.GetBoardIndex() == boardIndex
                && move.GetRow() == row
                && move.GetColumn() == column
                && move.GetPiece().DisplayValue() == pieceValue
            )
            {
                return move;
            }
        }

        return null;
    }

    private static int ReadInt(JsonElement item, string propertyName)
    {
        if (
            !item.TryGetProperty(propertyName, out JsonElement value)
            || value.ValueKind != JsonValueKind.Number
            || !value.TryGetInt32(out int number)
        )
        {
            throw new InvalidDataException($"The save is missing a valid {propertyName} value.");
        }

        return number;
    }

    private static bool ReadBool(JsonElement item, string propertyName)
    {
        if (
            !item.TryGetProperty(propertyName, out JsonElement value)
            || (value.ValueKind != JsonValueKind.True && value.ValueKind != JsonValueKind.False)
        )
        {
            throw new InvalidDataException($"The save is missing a valid {propertyName} value.");
        }

        return value.GetBoolean();
    }

    private static string ReadString(JsonElement item, string propertyName)
    {
        if (
            !item.TryGetProperty(propertyName, out JsonElement value)
            || value.ValueKind != JsonValueKind.String
        )
        {
            throw new InvalidDataException($"The save is missing a valid {propertyName} value.");
        }

        string? text = value.GetString();

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidDataException($"The saved {propertyName} value cannot be empty.");
        }

        return text;
    }
} // closes saveManager class
