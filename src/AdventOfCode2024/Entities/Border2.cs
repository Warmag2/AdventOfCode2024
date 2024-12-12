using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Entities;

public struct Border2
{
    public Border2(Vertex2 inside, Vertex2 outside)
    {
        I = inside;
        O = outside;
    }

    public Vertex2 I { get; set; }

    public Vertex2 O { get; set; }

    public BorderDirection Alignment => GetAlignment();

    public static bool operator !=(Border2 a, Border2 b)
    {
        return a.I != b.I || a.O != b.O;
    }

    public static bool operator ==(Border2 a, Border2 b)
    {
        return a.I == b.I && a.O == b.O;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is Border2 border)
        {
            return Equals(border);
        }

        return false;
    }

    public readonly bool Equals(Border2 other)
    {
        return this == other;
    }

    public bool IsAdjacentAndInLine(Border2 other)
    {
        if (Alignment != other.Alignment)
        {
            return false;
        }

        switch (Alignment)
        {
            case BorderDirection.NorthSouth:
                return (I == other.I + Vertex2.DirectionVertex(Direction.North) && O == other.O + Vertex2.DirectionVertex(Direction.North)) ||
                    (I == other.I + Vertex2.DirectionVertex(Direction.South) && O == other.O + Vertex2.DirectionVertex(Direction.South));
            case BorderDirection.EastWest:
                return (I == (other.I + Vertex2.DirectionVertex(Direction.East)) && O == (other.O + Vertex2.DirectionVertex(Direction.East))) ||
                    (I == other.I + Vertex2.DirectionVertex(Direction.West) && O == other.O + Vertex2.DirectionVertex(Direction.West));
            default:
                throw new InvalidOperationException("Unknown BorderDirection");
        }
    }

    private BorderDirection GetAlignment()
    {
        // east-west
        if (I == O + Vertex2.DirectionVertex(Direction.North) ||
            I == O + Vertex2.DirectionVertex(Direction.South))
        {
            return BorderDirection.EastWest;
        }

        // north-south
        if (I == O + Vertex2.DirectionVertex(Direction.West) ||
            I == O + Vertex2.DirectionVertex(Direction.East))
        {
            return BorderDirection.NorthSouth;
        }

        throw new InvalidDataException("This is not a border.");
    }
}
