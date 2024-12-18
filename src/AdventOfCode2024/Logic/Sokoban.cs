using AdventOfCode2024.Entities;
using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Logic;

public class Sokoban
{
    private const char Impassable = '#';
    private const char Player = '@';
    private const char Rock = 'O';
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

    public Sokoban(Map2D<char> map)
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
        }

        // Rock or line of rocks
        if (nextSquare == Rock)
        {
            var freeSpace = GetNextFreeSpace(direction);

            if (freeSpace.HasValue)
            {
                Map.Set(freeSpace.Value, Rock);
                Map.Set(_playerPos, Floor);
                _playerPos += direction;
                Map.Set(_playerPos, Player);
            }
        }
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
