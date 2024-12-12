using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Extensions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 12)]
public class Problem12 : ProblemBase
{
    public Problem12(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var map = new Map2D<char>(Input.ToArray2D(['\r', '\n']));
        var blankMap = new Map2D<char>(map.SizeX, map.SizeY, '.');

        var areas = new List<List<Vertex2>>();

        while (true)
        {
            Console.WriteLine(blankMap.ToString());

            var startLocation = blankMap.FirstInstanceOf('.');

            if (!startLocation.HasValue)
            {
                break;
            }

            var fillArea = map.GetContiguousArea(startLocation.Value);
            blankMap.Set(fillArea, 'X');

            areas.Add(fillArea);
        }

        Console.WriteLine(blankMap.ToString());

        var checkSum = 0;
        var checkSumBonus = 0;

        foreach (var area in areas)
        {
            checkSum += area.Count * map.GetBorderLength(area);
            var sample = map.Get(area[0]);
            var borders = map.GetBorders(area);
            var sides = map.GetBorderSides(borders.ShallowClone());
            checkSumBonus += area.Count * sides;
        }

        return $"Solution: {checkSum} Bonus: {checkSumBonus} ";
    }
}
