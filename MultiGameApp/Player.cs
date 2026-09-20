namespace MultiGameApp;

public abstract class Player
{
    private readonly List<Piece> _availablePieces;

    protected Player(int id, string name, List<Piece> availablePieces)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Player name cannot be empty.");
        ArgumentNullException.ThrowIfNull(availablePieces);

        Id = id;
        Name = name;
        _availablePieces = [.. availablePieces];
    }

    public int Id { get; }

    public string Name { get; }

    public IReadOnlyList<Piece> AvailablePieces => _availablePieces;

    public bool HasPiece(Piece piece)
    {
        ArgumentNullException.ThrowIfNull(piece);
        return _availablePieces.Contains(piece);
    }

    public bool UsePiece(Piece piece)
    {
        if (!HasPiece(piece))
        {
            return false;
        }

        if (!piece.IsReusable())
        {
            _availablePieces.Remove(piece);
        }

        return true;
    }

    public void RestorePiece(Piece piece)
    {
        if (!piece.IsReusable() && !HasPiece(piece))
        {
            _availablePieces.Add(piece);
        }
    }

    public abstract bool IsComputer();
}

public class HumanPlayer(int id, string name, List<Piece> availablePieces)
    : Player(id, name, availablePieces)
{
    public override bool IsComputer()
    {
        return false;
    }
}

public class ComputerPlayer(int id, string name, List<Piece> availablePieces)
    : Player(id, name, availablePieces)
{
    // TO DO
    // add MoveStrategy
    // add ChooseMove(Game)
    // once implemented
    public override bool IsComputer()
    {
        return true;
    }
}
