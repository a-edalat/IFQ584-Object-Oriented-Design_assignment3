namespace MultiGameApp;

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

        // I assume the boards should have the same number of rows and columns
        // smarter thing is to move this check to each game, check if Xander has done this
        if (rows != cols)
        {
            throw new ArgumentException(nameof(rows));
        }

        this.rows = rows;
        this.cols = cols;
        cells = new int[rows * cols]; // array for number of cells

    }

    // Methods
    // - PlaceMove
    // - RemoveMove
    // - IsFull

    // decided to make IsWithinBoard private, as all my methods here are the ones
    // that are using it to check if they should progress
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
        if (IsCellEmpty(row, col) == true)
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
        if (IsCellEmpty(row, col) == false)
        {
            throw new InvalidOperationException();
        }

        int index = row * cols + col;
        cells[index] = 0;
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

    public void GetCell(int row, int col, int value)
    {
        int index = row * cols + col; //cols is the number of column in the board
        return cells[index];
    }

} // closes Board class
