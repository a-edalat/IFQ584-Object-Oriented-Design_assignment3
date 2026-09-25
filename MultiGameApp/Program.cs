using MultiGameApp;

internal static class Program
{
    private static void Main()
    {
        string savePath = Path.Combine(Environment.CurrentDirectory, "board-game-save.json");

        GameFactory gameFactory = new GameFactory();
        SaveManager saveManager = new SaveManager(savePath, gameFactory);
        Help help = new Help();
        GameController controller = new GameController(saveManager, help, gameFactory);

        controller.Run();
    }
}
