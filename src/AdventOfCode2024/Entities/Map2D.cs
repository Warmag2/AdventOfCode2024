﻿using AdventOfCode2024.Enums;
using static AdventOfCode2024.Extensions.CollectionExtensions;

namespace AdventOfCode2024.Entities;

public class Map2D<TType>
    where TType : struct, IComparable<TType>, IEquatable<TType>
{
    private readonly TType[][] _mapData;

    public Map2D(int sizeX, int sizeY, TType empty = default)
    {
        _mapData = new TType[sizeY][];

        for (int ii = 0; ii < _mapData.Length; ii++)
        {
            _mapData[ii] = new TType[sizeX];

            for (var jj = 0; jj < _mapData[ii].Length; jj++)
            {
                _mapData[ii][jj] = empty;
            }
        }
    }

    public Map2D(TType[][] input)
    {
        _mapData = input;
    }

    public int SizeX => _mapData[0].Length;

    public int SizeY => _mapData.Length;

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

    public Dictionary<TType, List<Vertex2>> GetNonEmptySquares(TType empty)
    {
        var result = new Dictionary<TType, List<Vertex2>>();

        for (var ii = 0; ii < _mapData.Length; ii++)
        {
            for (var jj = 0; jj < _mapData[ii].Length; jj++)
            {
                if (_mapData[ii][jj].CompareTo(empty) != 0)
                {
                    result.AddInstance(_mapData[ii][jj], new Vertex2(jj, ii));
                }
            }
        }

        return result;
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

    public (Direction? DirectionAt, int DistanceTo) IsAtDirectionFrom(Vertex2 a, Vertex2 b)
    {
        var directions = Enum.GetValues<Direction>();

        foreach (var direction in directions)
        {
            var diff = b - a;
            var normalizedDir = diff.NormalizeToDirection();

            if (normalizedDir == Vertex2.DirectionVertex(direction))
            {
                return (direction, Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y)));
            }
        }

        return (null, 0);
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

    public bool Set(Vertex2 pos, TType input)
    {
        if (pos.Y >= 0 &&
            pos.Y < _mapData.Length &&
            pos.X >= 0 &&
            pos.X < _mapData[pos.Y].Length)
        {
            _mapData[pos.Y][pos.X] = input;
            return true;
        }

        return false;
    }

    public void SetLine(Vertex2 initPos, Vertex2 step, TType input)
    {
        var next = initPos;

        while (Set(next, input))
        {
            next += step;
        }
    }

    public override string ToString()
    {
        return string.Join("\r\n", _mapData.Select(LineToString));
    }

    public List<List<Vertex2>> WalkSequence(Vertex2 location, TType[] sequence)
    {
        var finalPaths = new List<List<Vertex2>>();
        var currentPath = new List<Vertex2>() { location };

        WalkSequenceInternal(finalPaths, currentPath, sequence, 0);

        return finalPaths;
    }

    private static string LineToString(TType[] input)
    {
        var result = string.Empty;

        foreach (var item in input)
        {
            result += item.ToString();
        }

        return result;
    }

    private void WalkSequenceInternal(List<List<Vertex2>> finalPaths, List<Vertex2> currentPath, TType[] sequence, int sequenceIndex)
    {
        var currentPositionValue = Get(currentPath[^1]);

        if (currentPositionValue.HasValue && currentPositionValue.Equals(sequence[^1]))
        {
            finalPaths.Add(currentPath);
            return;
        }

        var nextSteps = GetNeighborsOfType(currentPath[^1], [Direction.North, Direction.East, Direction.South, Direction.West], sequence[sequenceIndex + 1]);

        // Optimization, avoid needless memory reservations
        for (var ii = 1; ii < nextSteps.Count; ii++)
        {
            var newPath = currentPath.ShallowClone();
            newPath.Add(nextSteps[ii]);
            WalkSequenceInternal(finalPaths, newPath, sequence, sequenceIndex + 1);
        }

        if (nextSteps.Count > 0)
        {
            currentPath.Add(nextSteps[0]);
            WalkSequenceInternal(finalPaths, currentPath, sequence, sequenceIndex + 1);
        }
    }

    private List<Vertex2> GetNeighborsOfType(Vertex2 location, Direction[] directions, TType item)
    {
        var neighbors = new List<Vertex2>(directions.Length);

        foreach (var direction in directions)
        {
            var test = location + Vertex2.DirectionVertex(direction);
            var testValue = Get(test);

            if (testValue.HasValue && testValue.Value.Equals(item))
            {
                neighbors.Add(test);
            }
        }

        return neighbors;
    }
}
