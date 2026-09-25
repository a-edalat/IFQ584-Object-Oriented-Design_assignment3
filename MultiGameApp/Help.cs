namespace MultiGameApp;

public class Help
{
    public void Display(Game? game)
    {
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("N - Start a new game");
        Console.WriteLine("L - Load a saved game");
        Console.WriteLine("M - Make the current player's move");
        Console.WriteLine("U - Undo one move");
        Console.WriteLine("R - Redo one move");
        Console.WriteLine("S - Save the game");
        Console.WriteLine("H - Show this help");
        Console.WriteLine("Q - Quit");
        Console.WriteLine("Enter C when asked for a move to cancel it.");
        Console.WriteLine("Board, row, and column numbers shown on screen start at 1.");

        if (game != null)
        {
            Console.WriteLine();
            Console.WriteLine(game.GetHelpText());
        }

        Console.WriteLine();
    }
} // closes Help Class
