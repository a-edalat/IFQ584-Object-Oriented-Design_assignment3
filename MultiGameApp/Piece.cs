using System;

namespace MultiGameApp; 

public abstract class Piece
{
    public abstract string DisplayValue();

    public abstract bool IsReusable();
}

public class NumberPiece : Piece
{
    private readonly int _value;

    public NumberPiece(int value)
    {
        _value = value;
    }

    public int Value
    {
        get { return _value; }
    }

    public override string DisplayValue()
    {
        string displayedNumber = _value.ToString();

        return displayedNumber;
    }

    public override bool IsReusable()
    {
        return false;
    }
}

public class MarkPiece : Piece
{
    private readonly string _symbol;

    // the game assigns the symbol during setup/construction
    // this validation prevents an invalid game configuration
    public MarkPiece(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("The symbol cannot be empty", nameof(symbol));
        }

        _symbol = symbol;
    }

    public string Symbol
    {
        get { return _symbol; }
    }

    public override string DisplayValue()
    {
        return _symbol;
    }

    public override bool IsReusable()
    {
        return true;
    }
}
