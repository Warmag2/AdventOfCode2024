using AdventOfCode2024.Attributes;
using AdventOfCode2024.Extensions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 5)]
public class Problem05 : ProblemBase
{
    public Problem05(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var lists = Input.ToStringLists()!;

        var rules = lists[0].ToIntLists('|');
        var pageOrders = lists[1].ToIntLists(',');

        var middleSum = 0;
        var middleSumIncorrect = 0;

        foreach (var pageOrder in pageOrders)
        {
            var valid = true;

            foreach (var rule in rules)
            {
                var left = pageOrder.IndexOf(rule[0]);
                var right = pageOrder.IndexOf(rule[1]);

                if (left != -1 && right != -1 && left > right)
                {
                    valid = false;
                    continue;
                }
            }

            if (valid)
            {
                middleSum += pageOrder[pageOrder.Count / 2];
            }
            else
            {
                var notYetFixed = true;

                while (notYetFixed)
                {
                    notYetFixed = false;

                    foreach (var rule in rules)
                    {
                        var left = pageOrder.IndexOf(rule[0]);
                        var right = pageOrder.IndexOf(rule[1]);

                        if (left != -1 && right != -1 && left > right)
                        {
                            pageOrder.Swap(left, right);
                            notYetFixed = true;
                        }
                    }
                }

                middleSumIncorrect += pageOrder[pageOrder.Count / 2];
            }
        }

        return $"Solution: {middleSum}\nBonus solution: {middleSumIncorrect}";
    }
}
