namespace MultiGameApp;

public class GameController
{
    // fields
    private Game? currentGame;
    private readonly SaveManager saveManager;
    private readonly Help help;
    private readonly GameFactory gameFactory;
    // private readonly string saveFilePath;

    // constructor
    public GameController(SaveManager saveManager, Help help, GameFactory gameFactory)
    {
        ArgumentNullException.ThrowIfNull(saveManager);
        ArgumentNullException.ThrowIfNull(help);
        ArgumentNullException.ThrowIfNull(gameFactory);

        this.saveManager = saveManager;
        this.help = help;
        this.gameFactory = gameFactory;
    }

    public void Run()
    {
        bool shouldExit = false;

        while (shouldExit == false)
        {
            if (currentGame == null)
            {
                Console.WriteLine("N - Start a new game");
                Console.WriteLine("L - Load a game");
                Console.WriteLine("H - Help");
                Console.WriteLine("Q - Quit");
            }
            else
            {
                DisplayGame();
                Console.WriteLine("M - Make a move");
                Console.WriteLine("U - Undo");
                Console.WriteLine("R - Redo");
                Console.WriteLine("S - Save");
                Console.WriteLine("N - Start a new game");
                Console.WriteLine("L - Load a game");
                Console.WriteLine("H - Help");
                Console.WriteLine("Q - Quit");
            }

            string? input = Console.ReadLine();

            if (input is null)
            {
                return;
            }

            string command = input.Trim().ToUpperInvariant();

            if (command == "Q")
            {
                return;
            }

            try
            {
                HandleCommand(command);
            }
            catch (EndOfStreamException)
            {
                return;
            }

        }
    }

    public void StartNewGame()
    {
        string gameType = SelectGameType();
        string gameMode = SelectGameMode();
        int boardSize = SelectBoardSize(gameType);

        currentGame = CreateGame(
            gameType,
            gameMode,
            boardSize);

        Console.WriteLine("A new game has been created.");
    }

    public void LoadGame()
    {
        if (saveManager.FileExists() == false)
        {
            Console.WriteLine("There is no saved game at that path.");
            return;
        }

        currentGame = saveManager.LoadGame();

        Console.WriteLine("The game was loaded.");
    }

    public string SelectGameType()
    {
        while (true)
        {
            Console.WriteLine("Select a game type:");
            Console.WriteLine("1 - Numerical Tic-Tac-Toe");
            Console.WriteLine("2 - Notakto");
            Console.WriteLine("3 - Gomoku");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "1")
            {
                return "NumericalTicTacToe";
            }

            if (choice == "2")
            {
                return "Notakto";
            }

            if (choice == "3")
            {
                return "Gomoku";
            }

            Console.WriteLine("That is not a valid game type.");
        }
    }

    public string SelectGameMode()
    {
        while (true)
        {
            Console.WriteLine("Select a game mode:");
            Console.WriteLine("1 - Human versus human");
            Console.WriteLine("2 - Human versus computer");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "1")
            {
                return "HumanVsHuman";
            }

            if (choice == "2")
            {
                return "HumanVsComputer";
            }

            Console.WriteLine("That is not a valid game mode.");
        }
    }

    public int SelectBoardSize(string gameType)
    {
        if (gameType == "Notakto")
        {
            return NotaktoGame.BoardSize;
        }

        if (gameType == "Gomoku")
        {
            return GomokuGame.BoardSize;
        }

        while (true)
        {
            Console.WriteLine(
                "Enter the board size. It must be at least 3:");

            string input = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(input, out int boardSize)
                && boardSize >= 3)
            {
                return boardSize;
            }

            Console.WriteLine(
                "Please enter a whole number of at least 3.");
        }
    }

    public Game CreateGame(string gameType, string gameMode, int boardSize)
    {
        GameType selectedGameType;
        GameMode selectedGameMode;

        if (gameType == "NumericalTicTacToe")
        {
            selectedGameType =
                GameType.NUMERICAL_TIC_TAC_TOE;
        }
        else if (gameType == "Notakto")
        {
            selectedGameType = GameType.NOTAKTO;
        }
        else if (gameType == "Gomoku")
        {
            selectedGameType = GameType.GOMOKU;
        }
        else
        {
            throw new ArgumentException(
                "The game type is not recognised.",
                nameof(gameType));
        }

        if (gameMode == "HumanVsHuman")
        {
            selectedGameMode = GameMode.HUMAN_VS_HUMAN;
        }
        else if (gameMode == "HumanVsComputer")
        {
            selectedGameMode = GameMode.HUMAN_VS_COMPUTER;
        }
        else
        {
            throw new ArgumentException(
                "The game mode is not recognised.",
                nameof(gameMode));
        }

        return gameFactory.CreateGame(
            selectedGameType,
            selectedGameMode,
            boardSize);
    }

    private static Move? FindHumanMove(Game game, int boardNumber, int rowNumber, int columnNumber, string? value)
    {
        int boardIndex = boardNumber - 1;
        int rowIndex = rowNumber - 1;
        int columnIndex = columnNumber - 1;

        IEnumerable<Move> validMoves = game.GetValidMoves();

        foreach (Move move in validMoves)
        {
            bool matchesBoard = move.GetBoardIndex() == boardIndex;
            bool matchesRow = move.GetRow() == rowIndex;
            bool matchesColumn = move.GetColumn() == columnIndex;
            bool matchesValue = value is null || move.GetPiece().DisplayValue() == value;

            if (matchesBoard && matchesRow && matchesColumn & matchesValue)
            {
                return move;
            }
        }

        return null;
    }

    private static Move? ReadHumanMove(Game game)
    {
        bool multiBoard = game.Boards.Count > 1;
        bool needsNumber = game is NumericalTTTGame; // this can be improved with each game class declaring their requirements rather than hardcoded here

        if (multiBoard)
        {
            Console.WriteLine("Enter: board row column (or C to cancel"); // check if c is implemented
        }
        else if (needsNumber)
        {
            Console.WriteLine("Enter: row column number (or C to cancel");
        }
        else
        {
            Console.WriteLine("Enter: row column (or C to cancel");
        }

        string? input = Console.ReadLine(); // user input here

        if (input is null)
        {
            return null;
        }

        if (input.Trim().Equals("C", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries); // converting space delimated to a list

        int expected;

        if (multiBoard || needsNumber)
        {
            expected = 3;
        }
        else
        {
            expected = 2;
        }

        if (parts.Length != expected)
        {
            Console.WriteLine("Wrong number of values. See the prompt above. Separate your input with one space.");
        }

        int offset;

        if (multiBoard)
        {
            offset = 1;
        }
        else
        {
            offset = 0;
        }

        int row;
        int column;

        if (multiBoard && !int.TryParse(parts[0], out _))
        {
            Console.WriteLine("Use whole numbers for positions and values.");
            return null;
        }

        if (!int.TryParse(parts[offset], out row))
        {
            Console.WriteLine("Use whole numbers for positions and values.");
            return null;
        }

        if (!int.TryParse(parts[offset + 1], out column))
        {
            Console.WriteLine("Use whole numbers for positions and values.");
            return null;
        }

        if (needsNumber && !int.TryParse(parts[2], out _))
        {
            Console.WriteLine("Use whole numbers for positions and values.");
            return null;
        }

        int board;

        if (multiBoard)
        {
            board = int.Parse(parts[0]);
        }
        else
        {
            board = 1;
        }

        string? value;

        if (needsNumber)
        {
            value = parts[2];
        }
        else
        {
            value = null;
        }

        Move? move = FindHumanMove(game, board, row, column, value);

        if (move is null)
        {
            Console.WriteLine("That position or piece is unavailable.");
        }
        return move;
    }

    private void MakeCurrentPlayerMove()
    {
        if (currentGame is null || currentGame.IsGameOver)
        {
            Console.WriteLine("Start a new game to continue.");
            return;
        }

        Player player = currentGame.GetCurrentPlayer();
        Move? move;

        if (player is ComputerPlayer computer)
        {
            move = computer.ChooseMove(currentGame);
        }
        else
        {
            move = ReadHumanMove(currentGame); //null means invalid move
        }

        if (move is null) return;

        bool moveWasSuccessful = currentGame.MakeMove(move);

        if (moveWasSuccessful)
        {
            Console.WriteLine($"{player.Name} played {move.GetPiece().DisplayValue()}");
        }
        else
        {
            Console.WriteLine("That move is not legal. Try again.");
        }
    }

    public void HandleCommand(string command)
    {
        if (command == "N")
        {
            StartNewGame();
            return;
        }

        if (command == "L")
        {
            LoadGame();
            return;
        }

        if (command == "H")
        {
            help.Display(currentGame);
            return;
        }

        if (currentGame == null)
        {
            Console.WriteLine("Start or load a game first.");
            return;
        }

        if (command == "M")
        {
            MakeCurrentPlayerMove();
        }
        else if (command == "U")
        {
            if (currentGame.UndoMove() == false)
            {
                Console.WriteLine(
                    "There are no moves available to undo.");
            }
        }
        else if (command == "R")
        {
            if (currentGame.RedoMove() == false)
            {
                Console.WriteLine(
                    "There are no moves available to redo.");
            }
        }
        else if (command == "S")
        {
            saveManager.SaveGame(currentGame);
            Console.WriteLine("The game was saved.");
        }
        else
        {
            Console.WriteLine("That command is not recognised.");
        }

        if (currentGame.IsGameOver)
        {
            DisplayResult();
        }
    }

    public void DisplayGame()
    {
        if (currentGame == null)
        {
            Console.WriteLine("There is no current game.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Game: {currentGame.GetType().Name}");

        DisplayBoards(currentGame);

        Console.WriteLine($"Current player: {currentGame.GetCurrentPlayer().Name}");

        Console.WriteLine($"Result: {currentGame.Result}");

        Console.WriteLine($"Valid moves available: " + $"{currentGame.GetValidMoves().Count}");

        Console.WriteLine();
    }

    public void DisplayResult()
    {
        if (currentGame == null)
        {
            return;
        }
        Console.WriteLine(
            $"Game result: {currentGame.Result}");
    }

    private static void DisplayBoards(Game game)
    {
        for (int boardIndex = 0; boardIndex < game.Boards.Count; boardIndex++)
        {
            Board board = game.Boards[boardIndex];
            (int Rows, int Columns) dimensions = board.GetDimensions();
            int cellWidth = Math.Max(3, (dimensions.Rows * dimensions.Columns).ToString().Length + 1);

            if (game is NotaktoGame notakto && notakto.InactiveBoards.Contains(boardIndex))
            {
                Console.WriteLine($"Board {boardIndex + 1} (inactive)");
            }
            else
            {
                Console.WriteLine($"Board {boardIndex + 1}");
            }

            Console.Write(" ".PadLeft(cellWidth));

            for (int column = 0; column < dimensions.Columns; column++)
            {
                Console.Write((column + 1).ToString().PadLeft(cellWidth));
            }

            Console.WriteLine();

            for (int row = 0; row < dimensions.Rows; row++)
            {
                Console.Write((row + 1).ToString().PadLeft(cellWidth));

                for (int column = 0; column < dimensions.Columns; column++)
                {
                    Piece? piece = board.GetCell(row, column);
                    string cellValue = piece is null ? "." : piece.DisplayValue();
                    Console.Write(cellValue.PadLeft(cellWidth));
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
} // closes GameController Class
