using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Extensions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 19)]
public class Problem19 : ProblemBase
{
    public Problem19(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var input = Input.ToStringLists();
        var tokens = input[0][0].Split(", ");
        tokens = tokens.OrderBy(x => -x.Length).ToArray(); // longest first!
        var patterns = input[1].Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        var successes = 0L;
        var successesBonus = 0L;

        //var tokenSolutionDict = GetMutualTokenLists(tokens);
        /*var uniqueTokens = GetUniqueTokenLists(tokens);

        foreach (var pattern in patterns)
        {
            var solution = FillPattern(tokens, pattern);

            if (solution.Any())
            {
                Console.WriteLine($"{pattern}: {solution.Count} solutions - {SolutionToString(solution)}");
                //successes++;
                //successesBonus += solution.Count;
            }
            else
            {
                Console.WriteLine($"{pattern}: none");
            }
        }*/

        /*foreach (var pattern in patterns)
        {
            var solution = FillPatternTree(tokenSolutionDict, tokens, pattern);

            if (solution != 0)
            {
                Console.WriteLine($"{pattern}: {solution}");
                successes++;
                successesBonus += solution;
            }
            else
            {
                Console.WriteLine($"{pattern}: none");
            }
        }*/

        foreach (var pattern in patterns)
        {
            var solution = FillPatternCountOnly(tokens, pattern);

            if (solution != 0)
            {
                Console.WriteLine($"{pattern}: {solution}");
                successes++;
                successesBonus += solution;
            }
            else
            {
                Console.WriteLine($"{pattern}: none");
            }
        }

        return $"Solution: {successes} Bonus: {successesBonus}";
    }

    private string[] ReduceAmountOfTokens(string[] tokens)
    {
        var nextIterationOfTokens = new List<string>();
        var currentTokens = tokens.ShallowClone();

        while (true)
        {
            foreach (var token in currentTokens)
            {
                var solution = FillPattern(currentTokens.Where(t => t != token).ToArray(), token);

                if (!solution.Any())
                {
                    nextIterationOfTokens.Add(token);
                }
                else
                {
                    Console.WriteLine($"Removing token: {token} - It has the following solutions: {SolutionToString(solution)}");
                }
            }

            if (currentTokens.Length == nextIterationOfTokens.Count)
            {
                break;
            }

            Console.WriteLine($"Current tokens left: {nextIterationOfTokens.Count}");

            currentTokens = nextIterationOfTokens.ToArray();
            nextIterationOfTokens.Clear();
        }

        return currentTokens;
    }

    private Dictionary<string, List<List<string>>> GetMutualTokenLists(string[] tokens)
    {
        Dictionary<string, List<List<string>>> tokenMutualCounts = new();

        /*var nextIterationOfTokens = new List<string>();
        var currentTokens = tokens.ShallowClone();*/

        foreach (var token in tokens)
        {
            var solution = FillPattern(tokens.Where(t => t != token).ToArray(), token);

            if (solution.Count > 0)
            {
                if (!tokenMutualCounts.ContainsKey(token))
                {
                    tokenMutualCounts.Add(token, solution);
                }
            }
        }

        return tokenMutualCounts;
    }

    private List<string> GetUniqueTokenLists(string[] tokens)
    {
        var uniqueTokens = new List<string>();

        foreach (var token in tokens)
        {
            var solution = FillPattern(tokens.Where(t => t != token).ToArray(), token);

            if (solution.Count == 0)
            {
                uniqueTokens.Add(token);
                Console.WriteLine($"Unique token: {token}");
            }
        }

        return uniqueTokens;
    }

    private List<List<string>> FillPattern(string[] tokens, string pattern)
    {
        var solutions = new List<List<string>>();

        var applicableTokens = tokens.Where(pattern.Contains).ToArray();

        FillPatternInternal(solutions, new List<string>(), applicableTokens, pattern);

        return solutions;
    }

    private void FillPatternInternal(List<List<string>> solutions, List<string> currentSolution, string[] tokens, string remainingPattern)
    {
        /*if (solutions.Count > 0)
        {
            return;
        }*/

        if (string.IsNullOrEmpty(remainingPattern))
        {
            solutions.Add(currentSolution);
        }

        foreach (var token in tokens)
        {
            if (remainingPattern.StartsWith(token))
            {
                var newCurrentSolution = currentSolution.ShallowClone();
                newCurrentSolution.Add(token);
                FillPatternInternal(solutions, newCurrentSolution, tokens.Where(remainingPattern.Contains).ToArray(), remainingPattern.Substring(token.Length));
            }
        }
    }

    private long FillPatternCountOnly(string[] tokens, string pattern)
    {
        var knownPatterns = new Dictionary<string, long>();

        var solution = FillPatternInternalCountOnly(knownPatterns, tokens, pattern);

        return solution;
    }

    private long FillPatternInternalCountOnly(Dictionary<string, long> knownPatterns, string[] tokens, string remainingPattern)
    {
        if (knownPatterns.TryGetValue(remainingPattern, out var value))
        {
            return value;
        }

        var solution = 0L;

        var applicableTokens = tokens.Where(remainingPattern.Contains).ToArray();

        foreach (var token in applicableTokens)
        {
            if (remainingPattern.StartsWith(token))
            {
                if (remainingPattern.Length > token.Length)
                {
                    solution += FillPatternInternalCountOnly(knownPatterns, applicableTokens, remainingPattern.Substring(token.Length));
                }
                else
                {
                    solution++;
                }
            }
        }

        knownPatterns.TryAdd(remainingPattern, solution);

        return solution;
    }

    private long FillPatternTree(Dictionary<string, List<List<string>>> knownPatterns, string[] tokens, string pattern)
    {
        var applicableTokens = tokens.Where(pattern.Contains).ToArray();

        var root = new NTree() { Cost = 1 };

        FillPatternInternalTree(knownPatterns, root, applicableTokens, pattern);

        return root.GetSubTreeCost();
    }

    private void FillPatternInternalTree(Dictionary<string, List<List<string>>> knownPatterns, NTree tree, string[] tokens, string remainingPattern)
    {
        bool hit = false;

        foreach (var token in tokens)
        {
            if (remainingPattern.StartsWith(token))
            {
                hit = true;

                NTree subTree;

                if (knownPatterns.TryGetValue(token, out var patternList))
                {
                    var prunes = 0;

                    foreach (var pattern in patternList)
                    {
                        // Prune all starting patterns we now know not to exist
                        if (tree.Prune(pattern))
                        {
                            prunes++;
                        }
                    }

                    if (prunes > 0)
                    {
                        subTree = tree.TryAddForce(token, prunes + 1);
                    }
                    else
                    {
                        subTree = tree.TryAdd(token, 1);
                    }
                }
                else
                {
                    subTree = tree.TryAdd(token, 1);
                }

                if (subTree.Cost != 0 && remainingPattern.Length > token.Length)
                {
                    FillPatternInternalTree(knownPatterns, subTree, tokens, remainingPattern.Substring(token.Length));
                }
            }
        }

        // Dead end, prune this branch
        if (!hit)
        {
            tree.Cost = 0;
        }
    }

    private string SolutionToString(List<List<string>> solutions)
    {
        return string.Join(", ", solutions.Select(s => $"({string.Join(" ", s)})"));
    }
}
