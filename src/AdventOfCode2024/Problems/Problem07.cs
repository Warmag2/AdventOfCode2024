using AdventOfCode2024.Attributes;
using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 7)]
public class Problem07 : ProblemBase
{
    private static readonly Dictionary<char, Direction> GuardInitDirection = new Dictionary<char, Direction>() { { '^', Direction.North }, { '>', Direction.East }, { 'v', Direction.South }, { '<', Direction.West } };

    public Problem07(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var equationsAndSolutions = Input.Split("\r\n");

        var results = new List<long>(equationsAndSolutions.Length);
        var constituents = new List<List<long>>(equationsAndSolutions.Length);

        foreach (var item in equationsAndSolutions)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                var intermediary = item.Split(':');
                results.Add(long.Parse(intermediary[0]));
                constituents.AddRange(intermediary[1].Trim().Split().Select(long.Parse).ToList());
            }
        }

        long checkSum = 0;

        for (var ii = 0; ii < results.Count; ii++)
        {
            var permutations = Permutations(['+', '*', '|'], constituents[ii].Count - 1);

            foreach (var permutation in permutations.Select(p => p.ToArray()))
            {
                var hypothericalResult = Calc(permutation, constituents[ii]);

                if (hypothericalResult == results[ii])
                {
                    checkSum += hypothericalResult;
                    break;
                }
            }
        }

        return $"Solution: {checkSum}\nBonus solution: {"urp"}";
    }

    private long Calc(char[] v, List<long> constituents)
    {
        var intermediaryResult = constituents[0];

        for (var ii = 0; ii < v.Length; ii++)
        {
            intermediaryResult = Calc(v[ii], intermediaryResult, constituents[ii + 1]);
        }

        return intermediaryResult;
    }

    private long Calc(char operatorChar, long a, long b)
    {
        switch (operatorChar)
        {
            case '+': return a + b;
            case '*': return a * b;
            case '|': return long.Parse($"{a}{b}");
            default: throw new NotImplementedException("Unimplemented operator.");
        }
    }

    public static List<string> Permutations(char[] permuters, int permutationLength)
    {
        var result = new List<string>();

        GetPermutationInner(result, "", permuters, permutationLength);

        return result;
    }

    private static void GetPermutationInner(List<string> permutations, string permutation, char[] permuters, int permutationLength)
    {
        if (permutationLength == 0)
        {
            permutations.Add(permutation);
            return;
        }

        foreach (var item in permuters)
        {
            GetPermutationInner(permutations, permutation + item, permuters, permutationLength - 1);
        }
}
}
