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
            throw new ArgumentException(nameof(rows));
        }

        if (cols < 3)
        {
            throw new ArgumentException(nameof(cols));
        }

        this.rows = rows;
        this.cols = cols;
        cells = new Piece?[rows * cols];
    }

    // Methods

    // used for Numerical TTT as it has variable board size.
    public bool IsWithinBoard(int row, int col)
    {
        if (row >= rows || row < 0)
        {
            return false;
        }
        if (col >= cols || col < 0)
        {
            return false;
        }
        return true;
    }

    // used for Gomoku, Notakto as they have fixed board size.
    public bool HasSize(int expectedRows, int expectedColumns)
    {
        return rows == expectedRows && cols == expectedColumns;
    }

    public bool IsCellEmpty(int row, int col)
    {
        if (IsWithinBoard(row, col) == false)
        {
            throw new ArgumentOutOfRangeException();
        }
        return GetCell(row, col) is null;
    }

    public void PlaceMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move);
        ArgumentNullException.ThrowIfNull(move.Piece); //Making sure move is not null

        // first extracting row, column and value
        int row = move.GetRow();
        int col = move.GetColumn();
        Piece val = move.GetValue();

        if (IsWithinBoard(row, col) == false)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (IsCellEmpty(row, col) == false)
        {
            throw new InvalidOperationException();
        }

        // int index = row * cols + col;
        // cells[index] = val;
        SetCell(row, col, val);
    }

    public void RemoveMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move);
        ArgumentNullException.ThrowIfNull(move.Piece); //Making sure move is not null

        // first extracting row, column and value
        int row = move.GetRow();
        int col = move.GetColumn();

        if (IsWithinBoard(row, col) == false)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (IsCellEmpty(row, col) == true)
        {
            throw new InvalidOperationException();
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
            throw new ArgumentOutOfRangeException();
        }
        return cells[row * cols + col];
    }
} // closes Board class
