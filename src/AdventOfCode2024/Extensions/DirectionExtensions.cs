using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Extensions;

public static class DirectionExtensions
{
    public static Direction Left(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.West;
            case Direction.NorthEast:
                return Direction.NorthWest;
            case Direction.East:
                return Direction.North;
            case Direction.SouthEast:
                return Direction.NorthEast;
            case Direction.South:
                return Direction.East;
            case Direction.SouthWest:
                return Direction.SouthEast;
            case Direction.West:
                return Direction.South;
            case Direction.NorthWest:
                return Direction.SouthWest;
            default:
                throw new NotImplementedException("Invalid direction.");
        }
    }

    public static Direction Right(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.East;
            case Direction.NorthEast:
                return Direction.SouthEast;
            case Direction.East:
                return Direction.South;
            case Direction.SouthEast:
                return Direction.SouthWest;
            case Direction.South:
                return Direction.West;
            case Direction.SouthWest:
                return Direction.NorthWest;
            case Direction.West:
                return Direction.North;
            case Direction.NorthWest:
                return Direction.NorthEast;
            default:
                throw new NotImplementedException("Invalid direction.");
        }
    }
}
