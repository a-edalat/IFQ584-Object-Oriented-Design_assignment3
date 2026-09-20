namespace MultiGameApp;

// Moving Display board to GameController or a UI class similar to what Xander was suggesting

public class Board
{
    // fields
    private int rows;
    private int cols;
    private int[] cells;

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
        cells = new int[rows * cols]; // array for number of cells
    }

    // Methods

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

    public bool IsCellEmpty(int row, int col)
    {
        if (IsWithinBoard(row, col) == false)
        {
            throw new ArgumentOutOfRangeException();
        }
        int index = row * cols + col;
        return cells[index] == 0;
    }

    public void PlaceMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move); //Making sure move is not null

        // first extracting row, column and value
        int row = move.GetRow();
        int col = move.GetColumn();
        int val = move.GetValue();

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
        ArgumentNullException.ThrowIfNull(move); //Making sure move is not null

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

        SetCell(row, col, 0);
    }

    public bool IsFull()
    {
        return Array.IndexOf(cells, 0) == -1; //check if cell with value zero is not found
    }

    private void SetCell(int row, int col, int value)
    {
        int index = row * cols + col; //cols is the number of column in the board
        cells[index] = value;
    }

    public int GetCell(int row, int col)
    {
        if (!IsWithinBoard(row, col))
        {
            throw new ArgumentOutOfRangeException();
        }
        int index = row * cols + col; //cols is the number of column in the board
        return cells[index];
    }
} // closes Board class
