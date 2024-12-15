using System.Text.RegularExpressions;
using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 13)]
public class Problem13 : ProblemBase
{
    public Problem13(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var problems = ParseProblem13(Input);

        var checksum = 0L;
        var checksumBonus = 0L;

        foreach (var problem in problems)
        {
            var solution = SolveVectorProblem(problem, false);
            checksum += solution.X * 3 + solution.Y;
            Console.WriteLine($"Solution: {solution.X} {solution.Y}");
            solution = SolveVectorProblem(problem, true);
            checksumBonus += solution.X * 3 + solution.Y;
            Console.WriteLine($"Bonus solution: {solution.X} {solution.Y}");
        }

        return $"Solution: {checksum} Bonus: {checksumBonus} ";
    }

    private static Vertex2 SolveVectorProblem(Vertex2[] problem, bool largeC)
    {
        var a1 = (decimal)problem[0].X;
        var a2 = (decimal)problem[0].Y;
        var b1 = (decimal)problem[1].X;
        var b2 = (decimal)problem[1].Y;
        var c1 = (decimal)problem[2].X;
        var c2 = (decimal)problem[2].Y;

        if (largeC)
        {
            c1 = c1 + 10000000000000m;
            c2 = c2 + 10000000000000m;
        }

        var y = (c1 * a2 - c2 * a1) / (b1 * a2 - a1 * b2);
        var x = (c2 - y * b2) / a2;

        if (x == Math.Round(x) && y == Math.Round(y))
        {
            if (!largeC && (x > 100 || y > 100))
            {
                return new Vertex2(0, 0);
            }

            return new Vertex2((long)x, (long)y);
        }
        else
        {
            return new Vertex2(0, 0);
        }
    }

    private List<Vertex2[]> ParseProblem13(string input)
    {
        var problems = new List<Vertex2[]>();
        var problem = new List<Vertex2>();

        var splitLines = input.Split("\r\n");

        var regex = new Regex("[0-9]+");

        for (var ii = 0; ii < splitLines.Count(); ii++)
        {
            problem.Clear();
            if (splitLines[ii].StartsWith("Button A: "))
            {
                var variables = regex.Matches(splitLines[ii]);
                problem.Add(new Vertex2(int.Parse(variables[0].ToString()), int.Parse(variables[1].ToString())));
                variables = regex.Matches(splitLines[ii+1]);
                problem.Add(new Vertex2(int.Parse(variables[0].ToString()), int.Parse(variables[1].ToString())));
                variables = regex.Matches(splitLines[ii + 2]);
                problem.Add(new Vertex2(int.Parse(variables[0].ToString()), int.Parse(variables[1].ToString())));
                problems.Add(problem.ToArray());
            }
        }

        return problems;
    }
}
