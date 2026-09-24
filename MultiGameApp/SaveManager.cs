using System.Text.Json;
namespace MultiGameApp;

// this class needs a careful review at the end to make sure it stores information sufficiently, currently it may not save all the necessary details yet
// I have not worked with JSON serializer yet, and it is difficult to imagine the outcome
//
// also check your SaveGame and LoadGame pathes to make sure they refer to the same path everytime, when you are testing above.

public class SaveManager
{
    // fields
    private readonly string filePath;

    // constructor
    public SaveManager(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "A file path is required.",
                nameof(filePath));
        }

        this.filePath = filePath;
    }

    // Methods
    public void SaveGame(Game game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        string gameType = game.GetType().Name;

        JsonElement gameData =
            JsonSerializer.SerializeToElement(
                game,
                game.GetType());

        SaveData saveData = new SaveData
        {
            GameType = gameType,
            Game = gameData
        };

        string json = JsonSerializer.Serialize(saveData);

        File.WriteAllText(filePath, json);
    }

    public Game LoadGame(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "A file path is required.",
                nameof(filePath));
        }
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "The save file could not be found.",
                filePath);
        }

        string json = File.ReadAllText(filePath);

        SaveData? saveData = JsonSerializer.Deserialize<SaveData>(json);

        if (saveData == null)
        {
            throw new InvalidOperationException(
                "The save file was invalid.");
        }

        string gameJson = saveData.Game.GetRawText();

        Game? loadedGame = null;

        if (saveData.GameType == "NumericalTicTacToe")
        {
            loadedGame =
                JsonSerializer.Deserialize<NumericalTicTacToe>(gameJson);
        }
        else if (saveData.GameType == "Notakto")
        {
            loadedGame =
                JsonSerializer.Deserialize<Notakto>(gameJson);
        }
        else if (saveData.GameType == "Gomoku")
        {
            loadedGame = JsonSerializer.Deserialize<Gomoku>(gameJson);
        }
        else
        {
            throw new InvalidOperationException(
                "The saved game type is not recognised.");
        }

        if (loadedGame == null)
        {
            throw new InvalidOperationException(
                "The game data could not be loaded.");
        }

        return loadedGame;
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    private class SaveData
    {
        public string GameType { get; set; } = string.Empty;

        public JsonElement Game { get; set; }
    }
} // closes saveManager class
