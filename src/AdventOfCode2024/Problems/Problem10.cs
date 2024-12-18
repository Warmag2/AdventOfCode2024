using AdventOfCode2024.Attributes;
using AdventOfCode2024.Helpers;
using AdventOfCode2024.Logic;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 10)]
public class Problem10 : ProblemBase
{
    public Problem10(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var map = new Map2D<char>(Input.ToArray2D(['\r', '\n']));

        var startLocations = map.GetInstancesOf('0');

        var totalPaths = 0;
        var totalPathsBonus = 0;

        foreach (var location in startLocations)
        {
            var paths = map.WalkSequence(location, "0123456789".ToArray());
            totalPathsBonus += paths.Count;

            //get last elements of paths and check for uniqueness
            var lastElements = paths.Select(p => p[^1]).ToList();
            var distinctElements = lastElements.Distinct().ToList();
            totalPaths += distinctElements.Count;
        }

        return $"Solution: {totalPaths} Bonus: {totalPathsBonus} ";
    }
}
