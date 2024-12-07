using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Entities;

public class Map2D<TType>
    where TType : struct, IComparable<TType>
{
    private readonly TType[][] _mapData;

    public Map2D(TType[][] input)
    {
        _mapData = input;
    }

    public TType? Get(Vertex2 location)
    {
        if (location.X >= 0 &&
            location.X < _mapData.Length &&
            location.Y >= 0 &&
            location.Y < _mapData[location.X].Length)
        {
            return _mapData[location.Y][location.X];
        }

        return null;
    }

    public List<Vertex2> GetInstancesOf(TType input)
    {
        var instances = new List<Vertex2>();

        for (var ii = 0; ii < _mapData.Length; ii++)
        {
            for (var jj = 0; jj < _mapData[ii].Length; jj++)
            {
                if (_mapData[ii][jj].CompareTo(input) == 0)
                {
                    instances.Add(new Vertex2 { X = jj, Y = ii });
                }
            }
        }

        return instances;
    }

    public Vertex2? FirstInstanceOf(TType input)
    {
        for (var ii = 0; ii < _mapData.Length; ii++)
        {
            for (var jj = 0; jj < _mapData[ii].Length; jj++)
            {
                if (_mapData[ii][jj].CompareTo(input) == 0)
                {
                    return new Vertex2 { X = jj, Y = ii };
                }
            }
        }

        return null;
    }

    public bool HasSequence(Vertex2 location, Direction direction, TType[] sequence)
    {
        int cursor = 0;

        do
        {
            var result = Get(location);

            if (!result.HasValue)
            {
                return false;
            }

            if (result.Value.CompareTo(sequence[cursor]) != 0)
            {
                return false;
            }

            cursor++;

            location = location.Travel(direction);
        }
        while (cursor < sequence.Length);

        return true;
    }

    public int MatchesOfSequenceInAnyDirection(Vertex2 location, TType[] sequence)
    {
        var matches = 0;

        foreach (var direction in Enum.GetValues<Direction>())
        {
            if (HasSequence(location, direction, sequence))
            {
                matches++;
            }
        }

        return matches;
    }

    public void Set(Vertex2 pos, TType input)
    {
        _mapData[pos.Y][pos.X] = input;
    }

    public string ToString()
    {
        return string.Join("\r\n", _mapData.Select(l => LineToString(l)));
    }

    private static string LineToString(TType[] input)
    {
        var result = "";

        foreach (var item in input)
        {
            result += item.ToString();
        }

        return result;
    }
}
