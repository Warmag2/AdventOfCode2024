using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Logic;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 18)]
public class Problem18 : ProblemBase
{
    public Problem18(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var size = 70; // Different for examples and actual, not parsable

        var map = new Map2D<char>(size + 1, size + 1);

        var blocks = ParseBlocks(Input);

        map.Reset('.');

        map.Set(blocks[..12], '#');

        map.Set(new Vertex2(0, 0), 'S');
        map.Set(new Vertex2(size, size), 'E');

        var maze = new Maze(map, 1, 0);
        var finalPaths = maze.Crawl(false);

        Console.WriteLine(map);

        map.Set(finalPaths[0], 'O');

        Console.WriteLine(map);

        var bonusMap = new Map2D<char>(size + 1, size + 1);
        bonusMap.Reset('.');
        bonusMap.Set(new Vertex2(0, 0), 'S');
        bonusMap.Set(new Vertex2(size, size), 'E');
        var bonusMaze = new Maze(bonusMap, 1, 0);

        for (var ii = 0; ii < blocks.Count; ii++)
        {
            bonusMap.Set(blocks[ii], '#');
            bonusMaze.Reset();
            var bonusFinalPaths = bonusMaze.Crawl(false);

            if (!bonusFinalPaths.Any())
            {
                Console.WriteLine(bonusMap);
                return $"Solution: {maze.GetCost()} Bonus: {$"{blocks[ii].X},{blocks[ii].Y}"}";
            }
        }

        return $"Solution: {maze.GetCost()} Bonus: {"urp"}";
    }

    private List<Vertex2> ParseBlocks(string input)
    {
        var blocks = new List<Vertex2>();

        foreach (var line in input.Split())
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                var numbers = line.Split(',');
                blocks.Add(new Vertex2(int.Parse(numbers[0]), int.Parse(numbers[1])));
            }
        }

        return blocks;
    }
}
