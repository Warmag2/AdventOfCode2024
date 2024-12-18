using AdventOfCode2024.Attributes;
using AdventOfCode2024.Helpers;
using AdventOfCode2024.Logic;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 16)]
public class Problem16 : ProblemBase
{
    public Problem16(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var map = new Map2D<char>(Input.ToArray2D(['\r', '\n']));

        var maze = new Maze(map, 1, 1000);
        var finalPaths = maze.Crawl();

        Console.WriteLine(map);

        map.Set(finalPaths.SelectMany(f => f), 'O');

        Console.WriteLine(map);

        return $"Solution: {maze.GetCost()} Bonus: {map.GetInstancesOf('O').Count}";
    }
}
