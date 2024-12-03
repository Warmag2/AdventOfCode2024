using AdventOfCode2024.Attributes;
using System.Collections.Concurrent;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 1)]
public class Problem_01 : IProblem
{
    readonly string Input;

    public Problem_01(string input)
    {
        Input = input;
    }

    public string Solve()
    {
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in Input.Split("\r\n"))
        {
            if (line.Length == 0)
            {
                continue;
            }

            var numbers = line.Split();
            left.Add(int.Parse(numbers.First()));
            right.Add(int.Parse(numbers.Last()));
        }

        left.Sort();
        right.Sort();

        // Part 1
        var diff = new List<int>();

        for (var ii = 0; ii < left.Count; ii++)
        {
            diff.Add(Math.Abs(right[ii] - left[ii]));
        }

        // Part 2
        var similarityScores = new List<int>();

        var similarityDict = new ConcurrentDictionary<int, int>();

        foreach (var item in right)
        {
            similarityDict.AddOrUpdate(item, 1, (_, existingValue) => existingValue + 1);
        }

        int similaritySum = 0;

        foreach (var item in left)
        {
            if (similarityDict.TryGetValue(item, out var value))
            {
                similaritySum += item * value;
            }
        }

        return $"Solution: {diff.Sum(x => x).ToString()}\nBonus solution: {similaritySum}";
    }
}
