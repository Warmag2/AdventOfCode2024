using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Entities;

public struct Vertex2
{
    public int X { get; set; }

    public int Y { get; set; }

    public static bool operator !=(Vertex2 a, Vertex2 b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    public static bool operator ==(Vertex2 a, Vertex2 b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static Vertex2 operator +(Vertex2 a, Vertex2 b)
    {
        return new Vertex2 { X = a.X + b.X, Y = a.Y + b.Y };
    }

    public static Vertex2 LocationAt(Vertex2 vertex, Direction direction)
    {
        return vertex + vertex.Travel(direction);
    }

    public Vertex2 Travel(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vertex2 { X = X, Y = Y - 1 };
            case Direction.NorthEast:
                return new Vertex2 { X = X + 1, Y = Y - 1 };
            case Direction.East:
                return new Vertex2 { X = X + 1, Y = Y };
            case Direction.SouthEast:
                return new Vertex2 { X = X + 1, Y = Y + 1 };
            case Direction.South:
                return new Vertex2 { X = X, Y = Y + 1 };
            case Direction.SouthWest:
                return new Vertex2 { X = X - 1, Y = Y + 1 };
            case Direction.West:
                return new Vertex2 { X = X - 1, Y = Y };
            case Direction.NorthWest:
                return new Vertex2 { X = X - 1, Y = Y - 1 };
            default:
                throw new NotImplementedException("Unknown direction.");
        }
    }
}
