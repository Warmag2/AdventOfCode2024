﻿using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Helpers;

public static class MapHelpers
{
    public static Direction TurnRight(this Direction direction)
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
                throw new NotImplementedException("Unknown direction.");
        }
    }
}