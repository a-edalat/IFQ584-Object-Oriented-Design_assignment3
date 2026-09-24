namespace MultiGameApp;

public class Move
{
    // fields
    private readonly Player player;
    private readonly int boardIndex;
    private readonly int row;
    private readonly int column;
    private readonly int value;

    // constructor
    public Move(Player player, int boardIndex, int row, int column, int value)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }
        if (boardIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(boardIndex));
        }
        if (row < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }
        if (column < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        this.player = player;
        this.boardIndex = boardIndex;
        this.row = row;
        this.column = column;
        this.value = value;
    }

    // Methods

    public Player GetPlayer()
    {
        return player;
    }
    public int GetBoardIndex()
    {
        return boardIndex;
    }
    public int GetRow()
    {
        return row;
    }
    public int GetColumn()
    {
        return column;
    }
    public int GetValue()
    {
        return value;
    }



} // closes Move class
