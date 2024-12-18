using AdventOfCode2024.Attributes;
using AdventOfCode2024.Logic;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 17)]
public class Problem17 : ProblemBase
{
    public Problem17(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var emulator = new Emulator2417(Input);

        var output = emulator.Run();

        long lowestInput = 0;

        var outputToCompareAgainst = emulator.Program.ToArray();
        List<int> outputBonus = new();
        var lastBest = 0;

        Console.WriteLine(string.Join(',', outputToCompareAgainst));

        while (true)
        {
            emulator.Ax = lowestInput;
            emulator.Bx = 0;
            emulator.Cx = 0;

            outputBonus = emulator.Run();

            Console.WriteLine(string.Join(',', outputBonus) + $" Ax: {lowestInput}");

            var quality = OutputQuality(outputBonus, outputToCompareAgainst);
            //Console.WriteLine(quality);

            if (quality == outputToCompareAgainst.Length)
            {
                break;
            }

            if (quality > lastBest)
            {
                lowestInput = lowestInput << 3;
                lastBest++;
            }
            else
            {
                lowestInput++;
            }

            //Console.ReadKey();
        }

        return $"Solution: {string.Join(',', output)} Bonus: {lowestInput} OutputBonus: {string.Join(',', outputBonus)}";
    }

    public int OutputQuality(IList<int> a, IList<int> b)
    {
        for (var ii = 0; ii < Math.Min(a.Count, b.Count); ii++)
        {
            if (a[a.Count - 1 - ii] != b[b.Count - 1 - ii])
            {
                return ii;
            }
        }

        return a.Count;
    }
}
