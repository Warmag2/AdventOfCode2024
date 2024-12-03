using AdventOfCode2024.Attributes;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 2)]
public class Problem_02 : IProblem
{
    readonly string Input;

    public Problem_02(string input)
    {
        Input = input;
    }

    public string Solve()
    {
        var reports = new List<List<int>>();

        foreach (var line in Input.Split("\r\n"))
        {
            if (line.Length == 0)
            {
                continue;
            }

            var numbers = line.Split();
            reports.Add(numbers.Select(int.Parse).ToList());
        }

        // Part 1
        var reportValidity = new List<bool>();

        foreach (var report in reports)
        {
            reportValidity.Add(CheckReport(report));
            Console.WriteLine($"{string.Join(' ', report)}: {reportValidity.Last()}");
        }

        // Part 2
        var reportValidity2 = new List<bool>();

        foreach (var report in reports)
        {
            if (CheckReport(report))
            {
                reportValidity2.Add(true);
            }
            else
            {
                bool solved = false;

                for (var ii = 0; ii < report.Count; ii++)
                {
                    var modifiedReport = report.ShallowClone();
                    modifiedReport.RemoveAt(ii);
                    if (CheckReport(modifiedReport))
                    {
                        reportValidity2.Add(true);
                        solved = true;
                        break;
                    }
                }

                if (!solved)
                {
                    reportValidity2.Add(false);
                }
            }

            Console.WriteLine($"{string.Join(' ', report)}: {reportValidity2.Last()}");
        }

        return $"Solution: {reportValidity.Sum(x => x == true ? 1 : 0)}\nBonus solution: {reportValidity2.Sum(x => x == true ? 1 : 0)}";
    }

    private bool CheckReport(List<int> report)
    {
        var directionVector = new List<int>();

        for (var ii = 1; ii < report.Count; ii++)
        {
            directionVector.Add(report[ii].CompareTo(report[ii - 1]));
        }

        for (var ii = 0; ii < directionVector.Count; ii++)
        {
            if (directionVector[ii] != 0)
            {
                directionVector[ii] = directionVector[ii] / Math.Abs(directionVector[ii]);
            }
        }

        if (!directionVector.All(x => x == directionVector[0]))
        {
            return false;
        }

        for (var ii = 1; ii < report.Count; ii++)
        {
            var change = Math.Abs(report[ii] - report[ii - 1]);
            if (change > 3 || change < 1)
            {
                return false;
            }
        }

        return true;
    }
}
