using AdventOfCode2024.Entities;
using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Logic;

public class SokobanLarge
{
    private const char Impassable = '#';
    private const char Player = '@';
    private const char RockLeft = '[';
    private const char RockRight = ']';
    private const char Floor = '.';

    private readonly Dictionary<char, Direction> DirectionMap = new()
    {
        { '^', Direction.North },
        { '>', Direction.East },
        { 'v', Direction.South },
        { '<', Direction.West }
    };

    private readonly Dictionary<char, Vertex2> DirectionVertexMap = new()
    {
        { '^', Vertex2.DirectionVertex(Direction.North) },
        { '>', Vertex2.DirectionVertex(Direction.East) },
        { 'v', Vertex2.DirectionVertex(Direction.South) },
        { '<', Vertex2.DirectionVertex(Direction.West) }
    };

    private Vertex2 _playerPos;

    public SokobanLarge(Map2D<char> map)
    {
        Map = map;
        _playerPos = map.FirstInstanceOf(Player) ?? throw new InvalidDataException("No player found.");
    }

    public Map2D<char> Map { get; set; }

    public void Step(char instruction)
    {
        var direction = DirectionVertexMap[instruction];

        var nextSquare = Map.Get(_playerPos + direction) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");

        // Impassable
        if (nextSquare == Impassable)
        {
            return;
        }

        // Empty square
        if (nextSquare == Floor)
        {
            Map.Set(_playerPos, Floor);
            _playerPos += direction;
            Map.Set(_playerPos, Player);

            return;
        }

        var instructionDirection = DirectionMap[instruction];

        // Rock or line of rocks
        if (nextSquare == RockLeft || nextSquare == RockRight)
        {
            if (instructionDirection == Direction.West || instructionDirection == Direction.East)
            {
                var freeSpace = GetNextFreeSpace(direction);

                if (freeSpace != null)
                {
                    var drawPosition = _playerPos + direction;

                    while (drawPosition != freeSpace)
                    {
                        DrawRock(drawPosition + direction, instructionDirection);
                        drawPosition = drawPosition + direction * 2;
                    }

                    Map.Set(_playerPos, Floor);
                    _playerPos += direction;
                    Map.Set(_playerPos, Player);
                }
            }
            else
            {
                if (TryPush(instructionDirection))
                {
                    Push(instructionDirection);
                    Map.Set(_playerPos, Floor);
                    _playerPos += direction;
                    Map.Set(_playerPos, Player);
                }
            }
        }
    }

    private void Push(Direction instructionDirection)
    {
        var rocks = GetAllRocks(instructionDirection);

        while (rocks.Any())
        {
            for (var ii = 0; ii < rocks.Count; ii++)
            {
                if (TryMoveRock(rocks[ii], instructionDirection))
                {
                    rocks.RemoveAt(ii);
                    break;
                }
            }
        }
    }

    private bool TryMoveRock(Vertex2[] rockPosition, Direction direction)
    {
        var nextPos = rockPosition[0] + Vertex2.DirectionVertex(direction);
        var nextPosRight = rockPosition[1] + Vertex2.DirectionVertex(direction);
        var nextSquare = Map.Get(nextPos) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");
        var nextSquareRight = Map.Get(nextPosRight) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");

        if (nextSquare == Floor && nextSquareRight == Floor)
        {
            Map.Set(nextPos, RockLeft);
            Map.Set(nextPosRight, RockRight);
            Map.Set(rockPosition[0], Floor);
            Map.Set(rockPosition[1], Floor);
            return true;
        }

        return false;
    }

    private List<Vertex2[]> GetAllRocks(Direction direction)
    {
        var rockList = new List<Vertex2[]>();

        GetAllRocksInner(rockList, _playerPos, direction);

        return rockList;
    }

    private void GetAllRocksInner(List<Vertex2[]> rockList, Vertex2 position, Direction direction)
    {
        var nextPosition = position + Vertex2.DirectionVertex(direction);

        var rockPosition = GetRockPosition(nextPosition);

        if (rockPosition.Length > 0 && !rockList.Any(r => r[0] == rockPosition[0]))
        {
            rockList.Add(rockPosition);

            var nextSquare = Map.Get(rockPosition[0] + Vertex2.DirectionVertex(direction)) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");
            var nextSquareRight = Map.Get(rockPosition[1] + Vertex2.DirectionVertex(direction)) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");

            if (nextSquare == RockLeft || nextSquare == RockRight)
            {
                GetAllRocksInner(rockList, rockPosition[0], direction);
            }

            if (nextSquareRight == RockLeft)
            {
                GetAllRocksInner(rockList, rockPosition[1], direction);
            }
        }
    }

    private void DrawRock(Vertex2 drawPosition, Direction direction)
    {
        switch (direction)
        {
            case Direction.West:
                Map.Set(drawPosition, RockRight);
                Map.Set(drawPosition + Vertex2.DirectionVertex(direction), RockLeft);
                break;
            case Direction.East:
                Map.Set(drawPosition, RockLeft);
                Map.Set(drawPosition + Vertex2.DirectionVertex(direction), RockRight);
                break;
            default: throw new NotImplementedException("Trying to draw rock incorrectly.");
        }
    }

    private bool TryPush(Direction direction)
    {
        var position = _playerPos + Vertex2.DirectionVertex(direction);
        var square = Map.Get(position) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");

        var rockPosition = GetRockPosition(position);

        return !BlockedInner(rockPosition, direction);
    }

    private bool BlockedInner(Vertex2[] rockPosition, Direction direction)
    {
        // If rock is not here
        if (rockPosition.Length != 2)
        {
            return false;
        }

        if (Map.Get(rockPosition[0] + Vertex2.DirectionVertex(direction)) == Impassable || Map.Get(rockPosition[1] + Vertex2.DirectionVertex(direction)) == Impassable)
        {
            return true;
        }

        if (Map.Get(rockPosition[0] + Vertex2.DirectionVertex(direction)) == Floor && Map.Get(rockPosition[1] + Vertex2.DirectionVertex(direction)) == Floor)
        {
            return false;
        }

        return BlockedInner(GetRockPosition(rockPosition[0] + Vertex2.DirectionVertex(direction)), direction) ||
               BlockedInner(GetRockPosition(rockPosition[1] + Vertex2.DirectionVertex(direction)), direction);
    }

    private Vertex2[] GetRockPosition(Vertex2 position)
    {
        var square = Map.Get(position) ?? throw new InvalidDataException("Invalid data, map cannot find square next to player.");

        if (square == RockLeft)
        {
            return [position, position + Vertex2.DirectionVertex(Direction.East)];
        }

        if (square == RockRight)
        {
            return [position + Vertex2.DirectionVertex(Direction.West), position];
        }

        return [];
    }

    private Vertex2? GetNextFreeSpace(Vertex2 direction)
    {
        char square;
        var pos = _playerPos;

        do
        {
            pos += direction;
            square = Map.Get(pos) ?? throw new InvalidDataException("Invalid data, square off-map.");

            if (square == Floor)
            {
                return pos;
            }
        }
        while (square != Impassable);

        return null;
    }
}
