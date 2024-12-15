using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Entities;

public struct Vertex2
{
    public Vertex2(long x, long y)
    {
        X = x;
        Y = y;
    }

    public long X { get; set; }

    public long Y { get; set; }

    public static Vertex2 operator +(Vertex2 a, Vertex2 b)
    {
        return new Vertex2(a.X + b.X, a.Y + b.Y);
    }

    public static Vertex2 operator -(Vertex2 a, Vertex2 b)
    {
        return new Vertex2(a.X - b.X, a.Y - b.Y);
    }

    public static Vertex2 operator *(Vertex2 a, long b)
    {
        return new Vertex2(a.X * b, a.Y * b);
    }

    public static Vertex2 operator /(Vertex2 a, long b)
    {
        return new Vertex2(a.X / b, a.Y / b);
    }

    public static bool operator !=(Vertex2 a, Vertex2 b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    public static bool operator ==(Vertex2 a, Vertex2 b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static Vertex2 DirectionVertex(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vertex2(0, -1);
            case Direction.NorthEast:
                return new Vertex2 (1, -1);
            case Direction.East:
                return new Vertex2 (1, 0);
            case Direction.SouthEast:
                return new Vertex2 (1, 1);
            case Direction.South:
                return new Vertex2 (0, 1);
            case Direction.SouthWest:
                return new Vertex2 (-1, 1);
            case Direction.West:
                return new Vertex2 (-1, 0);
            case Direction.NorthWest:
                return new Vertex2 (-1, -1);
            default:
                throw new NotImplementedException("Unknown direction.");
        }
    }

    public static Vertex2 AntiDirectionVertex(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vertex2(0, 1);
            case Direction.NorthEast:
                return new Vertex2(-1, 1);
            case Direction.East:
                return new Vertex2(-1, 0);
            case Direction.SouthEast:
                return new Vertex2(-1, -1);
            case Direction.South:
                return new Vertex2(0, -1);
            case Direction.SouthWest:
                return new Vertex2(1, -1);
            case Direction.West:
                return new Vertex2(1, 0);
            case Direction.NorthWest:
                return new Vertex2(1, 1);
            default:
                throw new NotImplementedException("Unknown direction.");
        }
    }

    public static Vertex2 LocationAt(Vertex2 vertex, Direction direction)
    {
        return vertex + vertex.Travel(direction);
    }

    public Vertex2? NormalizeToDirection()
    {
        var lenMax = Math.Max(X, Y);

        if (X % lenMax == 0 && Y % lenMax == 0)
        {
            return new Vertex2 { X = X / lenMax, Y = Y / lenMax };
        }

        return null;
    }

    public Vertex2 Travel(Direction direction)
    {
        return this + DirectionVertex(direction);

        /*switch (direction)
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
        }*/
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is Vertex2 vertex)
        {
            return Equals(vertex);
        }

        return false;
    }

    public readonly bool Equals(Vertex2 other)
    {
        return this == other;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return (int)(X * 32768 + Y);
    }
}
