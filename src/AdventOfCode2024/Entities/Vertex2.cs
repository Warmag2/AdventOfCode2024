using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Entities;

public struct Vertex2
{
    public int X { get; set; }

    public int Y { get; set; }

    public Vertex2 Travel(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vertex2 { X = X, Y = Y + 1 };
            case Direction.NorthEast:
                return new Vertex2 { X = X + 1, Y = Y + 1 };
            case Direction.East:
                return new Vertex2 { X = X + 1, Y = Y };
            case Direction.SouthEast:
                return new Vertex2 { X = X + 1, Y = Y - 1 };
            case Direction.South:
                return new Vertex2 { X = X, Y = Y - 1 };
            case Direction.SouthWest:
                return new Vertex2 { X = X - 1, Y = Y - 1 };
            case Direction.West:
                return new Vertex2 { X = X - 1, Y = Y };
            case Direction.NorthWest:
                return new Vertex2 { X = X - 1, Y = Y + 1 };
            default:
                throw new NotImplementedException("Unknown direction.");
        }
    }
}
