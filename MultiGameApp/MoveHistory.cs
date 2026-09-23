namespace MultiGameApp;

public class MoveHistory
{

    // fields
    private readonly List<Move> moves; // the list can have member added or removed, but the list itself stays as is
    private int currentMoveIndex;

    // constructor
    public MoveHistory(List<Move> moves, int currentMoveIndex)
    {

        if (moves == null)
        {
            throw new ArgumentNullException(nameof(moves));
        }

        if (currentMoveIndex < -1 ||
            currentMoveIndex >= moves.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(currentMoveIndex));
        }

        this.moves = new List<Move>(moves);
        this.currentMoveIndex = currentMoveIndex;
    }

    // Methods

    public void ClearRedoHistory()
    {
        int firstRedoIndex = currentMoveIndex + 1;

        if (firstRedoIndex < moves.Count)
        {
            int numberOfRedoMoves = moves.Count - firstRedoIndex;

            moves.RemoveRange(firstRedoIndex, numberOfRedoMoves);
        }
    }

    public void AddMove(Move move)
    {
        if (move == null)
        {
            throw new ArgumentNullException(nameof(move));
        }

        ClearRedoHistory(); // making sure if the current move is a redo, to clear the undo moves

        moves.Add(move); // adding new move to the list

        currentMoveIndex++;
    }

    public List<Move> GetMoves()
    {
        return new List<Move>(moves); //making sure a copy is returned, so original object cannot be modified by user
    }

    public int GetCurrentIndex()
    {
        return currentMoveIndex;
    }

    public Move UndoLastMove()
    {
        if (currentMoveIndex == -1)
        {
            throw new InvalidOperationException(
                "There are no moves available to undo.");
        }

        Move moveToUndo = moves[currentMoveIndex];
        currentMoveIndex--;
        return moveToUndo;
    }

    public Move RedoLastMove()
    {
        if (currentMoveIndex + 1 >= moves.Count)
        {
            throw new InvalidOperationException(
                "There are no moves available to redo.");
        }
        currentMoveIndex++;
        return moves[currentMoveIndex];
    }

} // closes MoveHistory class
