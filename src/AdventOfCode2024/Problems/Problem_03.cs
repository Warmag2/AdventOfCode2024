using AdventOfCode2024.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 3)]
public class Problem_03 : IProblem
{
    readonly string Input;

    public Problem_03(string input)
    {
        Input = input;
    }

    public string Solve()
    {
        var totalSum = MulSum(Input);

        var bonusStrings = Input.Split("don't()");

        long bonusSum = 0;

        // Case where the initial block is valid
        // In other cases the first block is not valid
        if (!Input.StartsWith("don't()"))
        {
            bonusSum += MulSum(bonusStrings[0]);
        }

        for (var ii = 1; ii < bonusStrings.Length; ii++)
        {
            if (bonusStrings[ii].StartsWith("do()"))
            {
                var validityFindingSegments = bonusStrings[ii].Split("do()");
                foreach (var innerSegment in validityFindingSegments)
                {
                    bonusSum += MulSum(innerSegment);
                }
            }
            else
            {
                var validityFindingSegments = bonusStrings[ii].Split("do()");

                for (var jj = 1; jj < validityFindingSegments.Length; jj++)
                {
                    bonusSum += MulSum(validityFindingSegments[jj]);
                }
            }
        }

        return $"Solution: {totalSum}\nBonus solution: {bonusSum}";
    }

    private long MulSum(string input)
    {
        long totalSum = 0;

        var regex = new Regex("mul\\([0-9]+,[0-9]+\\)");

        foreach (var match in regex.Matches(input))
        {
            var mul = match.ToString();
            Console.WriteLine(mul);
            var innerRegex = new Regex("[0-9]+");

            var integers = new List<int>();
            foreach (var innerMatch in innerRegex.Matches(mul ?? throw new InvalidDataException("Regex doing something weird here.")))
            {
                integers.Add(int.Parse(innerMatch.ToString() ?? throw new InvalidDataException("Doing something wrong here.")));
            }

            totalSum += integers.Aggregate((ii, jj) => ii * jj);
        }

        return totalSum;
    }
}
