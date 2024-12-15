using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 15)]
public class Problem15 : ProblemBase
{
    public Problem15(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var input = Input.ToStringLists();

        var map = new Map2D<char>(input[0].ToArray2D());
        var instructions = ParseInstructions(input[1]);

        var sokoban = new Sokoban(map);

        Console.WriteLine(map.ToString());

        foreach (var instruction in instructions)
        {
            //Console.WriteLine($"Next instruction: {instruction}");
            //Console.ReadLine();
            sokoban.Step(instruction);
            //Console.WriteLine(map.ToString());
        }

        var newMap = new Map2D<char>(Widen(input[0].ToArray2D()));

        var sokobanLarge = new SokobanLarge(newMap);

        Console.WriteLine(newMap.ToString());

        foreach (var instruction in instructions)
        {
            Console.WriteLine($"Next instruction: {instruction}");
            //Console.ReadLine();
            sokobanLarge.Step(instruction);
            Console.WriteLine(newMap.ToString());
        }

        return $"Solution: {map.GetInstancesOf('O').Aggregate(0L, (s, o) => s + o.X + 100 * o.Y)} Bonus: {newMap.GetInstancesOf('[').Aggregate(0L, (s, o) => s + o.X + 100 * o.Y)}";
    }

    private static char[][] Widen(char[][] chars)
    {
        var charArrays = new char[chars.Length][];

        for (var ii = 0; ii < chars.Length; ii++)
        {
            var widenedLine = new List<char>();

            for (var jj = 0; jj < chars[ii].Length; jj++)
            {
                switch (chars[ii][jj])
                {
                    case '.':
                        widenedLine.AddRange(['.', '.']);
                        break;
                    case '#':
                        widenedLine.AddRange(['#', '#']);
                        break;
                    case 'O':
                        widenedLine.AddRange(['[', ']']);
                        break;
                    case '@':
                        widenedLine.AddRange(['@', '.']);
                        break;
                }
            }

            charArrays[ii] = widenedLine.ToArray();
        }

        return charArrays;
    }

    private static char[] ParseInstructions(List<string> list)
    {
        return list.SelectMany(x => x).ToArray();
    }
}
