using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Enums;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 4)]
public class Problem04 : ProblemBase
{
    public Problem04(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var map = new Map2D<char>(Input.ToArray2D(['\r', '\n']));

        var locations = map.GetInstancesOf('X');

        var hits = 0;

        foreach (var location in locations)
        {
            hits += map.MatchesOfSequenceInAnyDirection(location, "XMAS".ToArray());
        }

        var locationsBonus = map.GetInstancesOf('A');

        var hitsBonus = 0;

        foreach (var location in locationsBonus)
        {
            // This is idiotic but I can't be bothered because there are not too many cases to enumerate
            if (
                (map.HasSequence(location, Direction.NorthEast, "AS".ToArray()) &&
                map.HasSequence(location, Direction.SouthWest, "AM".ToArray()) &&
                map.HasSequence(location, Direction.NorthWest, "AS".ToArray()) &&
                map.HasSequence(location, Direction.SouthEast, "AM".ToArray()))
                ||
                (map.HasSequence(location, Direction.NorthEast, "AM".ToArray()) &&
                map.HasSequence(location, Direction.SouthWest, "AS".ToArray()) &&
                map.HasSequence(location, Direction.NorthWest, "AM".ToArray()) &&
                map.HasSequence(location, Direction.SouthEast, "AS".ToArray()))
                ||
                (map.HasSequence(location, Direction.NorthEast, "AS".ToArray()) &&
                map.HasSequence(location, Direction.SouthWest, "AM".ToArray()) &&
                map.HasSequence(location, Direction.NorthWest, "AM".ToArray()) &&
                map.HasSequence(location, Direction.SouthEast, "AS".ToArray()))
                ||
                (map.HasSequence(location, Direction.NorthEast, "AM".ToArray()) &&
                map.HasSequence(location, Direction.SouthWest, "AS".ToArray()) &&
                map.HasSequence(location, Direction.NorthWest, "AS".ToArray()) &&
                map.HasSequence(location, Direction.SouthEast, "AM".ToArray())))
            {
                hitsBonus++;
            }
        }

        return $"Solution: {hits}\nBonus solution: {hitsBonus}";
    }
}
