namespace MultiGameApp;

// Moving Display board to GameController or a UI class similar to what Xander was suggesting

public class Board
{
    // fields
    private readonly int rows;
    private readonly int cols;
    private readonly Piece?[] cells;

    // constructor
    public Board(int rows, int cols)
    {
        // I assume the boards should not be smaller than 3 x 3
        if (rows < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(rows), "A board needs at least three rows.");
        }

        if (cols < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(cols), "A board needs at least three columns.");
        }

        this.rows = rows;
        this.cols = cols;
        cells = new Piece?[rows * cols];
    }

    // Methods

    // used for Numerical TTT as it has variable board size.
    public bool IsWithinBoard(int row, int col)
    {
        return (row < rows && row >= 0 && col >= 0 && col < cols);
    }

    // used for Gomoku, Notakto as they have fixed board size.
    public bool HasSize(int expectedRows, int expectedColumns)
    {
        return rows == expectedRows && cols == expectedColumns;
    }

    public bool IsCellEmpty(int row, int col)
    {
        return GetCell(row, col) is null;
    }

    public void PlaceMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move);

        // first extracting row, column and value
        int row = move.GetRow();
        int col = move.GetColumn();

        if (!IsWithinBoard(row, col))
        {
            throw new ArgumentOutOfRangeException(nameof(move), "The move is outside the board.");
        }

        if (!IsCellEmpty(row, col))
        {
            throw new InvalidOperationException("The cell is already occupied.");
        }

        // int index = row * cols + col;
        // cells[index] = val;
        SetCell(row, col, move.GetPiece());
    }

    public void RemoveMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move);

        int row = move.GetRow();
        int col = move.GetColumn();

        if (!IsWithinBoard(row, col))
        {
            throw new ArgumentOutOfRangeException(nameof(move), "The move is outside the board.");
        }

        // Undo must not erase a different piece if board and history disagree.
        if (!ReferenceEquals(GetCell(row, col), move.GetPiece()))
        {
            throw new InvalidOperationException("The cell does not contain this move's piece.");
        }

        SetCell(row, col, null);
    }

    public bool IsFull()
    {
        return Array.IndexOf(cells, null) < 0;
    }

    private void SetCell(int row, int col, Piece? piece)
    {
        cells[row * cols + col] = piece;
    }

    public Piece? GetCell(int row, int col)
    {
        if (!IsWithinBoard(row, col))
        {
            throw new ArgumentOutOfRangeException("The position is outside the board.");
        }
        return cells[row * cols + col];
    }

    public (int Rows, int Columns) GetDimensions() // adding this method to make game display easier, this needs to be added to CRC and class diagram
    {
        return (rows, cols);
    }
} // closes Board class
