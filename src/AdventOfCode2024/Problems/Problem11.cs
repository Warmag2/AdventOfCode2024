using AdventOfCode2024.Attributes;
using AdventOfCode2024.Extensions;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 11)]
public class Problem11 : ProblemBase
{
    public Problem11(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var numbersList = Input.Trim().Split().Select(long.Parse).ToList();
        var numbers = new Dictionary<long, long>();

        foreach (var number in numbersList)
        {
            numbers.AddToCell(number, 1);
        }

        //var numbers = new Dictionary<long, long> { { 0, 1 } };

        var iterations = 0;

        while (iterations < 75)
        {
            //Console.WriteLine(GetNumberString(numbers));
            iterations++;
            numbers = Iterate(numbers);
            Console.WriteLine($"Iterations/Count: {iterations}/{numbers.Values.Sum()}");
        }

        //Console.WriteLine(GetNumberString(numbers));

        return $"Solution: {numbers.Values.Sum()} Bonus: {"urp"} ";
    }

    private static Dictionary<long, long> Iterate(Dictionary<long, long> numbers)
    {
        var newDict = new Dictionary<long, long>();

        foreach (var (number, amount) in numbers)
        {
            if (number == 0)
            {
                newDict.AddToCell(1, amount);
                continue;
            }

            var digits = (int)Math.Log10(number);

            if (digits >= 1 && digits % 2 == 1)
            {
                var splitPoint = digits / 2 + 1;
                var numberString = number.ToString();
                var numberLeft = long.Parse(numberString.Substring(0, splitPoint));
                var numberRight = long.Parse(numberString.Substring(splitPoint));
                newDict.AddToCell(numberLeft, amount);
                newDict.AddToCell(numberRight, amount);
                continue;
            }

            newDict.AddToCell(number * 2024, amount);
        }

        return newDict;
    }

    private string GetNumberString(List<long> numbers)
    {
        return string.Join(' ', numbers.Select(n => n.ToString()));
    }
}
