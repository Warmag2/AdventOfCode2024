using System.Diagnostics;
using System.Reflection;
using AdventOfCode2024.Attributes;
using CommandLine;
using static AdventOfCode2024.Helpers.TypeHelpers;

namespace AdventOfCode2024;

public class Program
{
    public class Options
    {
        [Option('p', "problem", Required = true, HelpText = "Problem number to process.")]
        public int Problem { get; set; }

        [Option('o', "overrideinput", Required = false, HelpText = "Override default input data file name with the given string.")]
        public string OverrideInputName { get; set; } = string.Empty;
    }

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(o =>
        {
            Console.WriteLine($"Running problem {o.Problem}");

            string inputFileName;

            if (!string.IsNullOrWhiteSpace(o.OverrideInputName))
            {
                inputFileName = o.OverrideInputName;
            }
            else
            {
                inputFileName = $"problem_{o.Problem:D2}.txt";
            }

            var inputData = File.ReadAllText(inputFileName);
            var problem = GetProblem(o.Problem, inputData);

            var watch = Stopwatch.StartNew();
            var result = problem.Solve();
            var finishTime = watch.Elapsed;

            Console.WriteLine(result);
            Console.WriteLine($"Time: {finishTime.TotalMilliseconds}ms");
        });
    }

    private static IProblem GetProblem(int requestedIndex, string inputData)
    {
        foreach (var implementation in GetTypesWithAttribute(Assembly.GetExecutingAssembly(), typeof(ProblemIndexAttribute)))
        {
            var index = (implementation.attribute as ProblemIndexAttribute)!.Index;

            if (requestedIndex == index)
            {
                return implementation.implementingType.GetConstructors().First().Invoke([inputData]) as IProblem ?? throw new InvalidOperationException("Fucked up something here.");
            }
        }

        throw new NotImplementedException("Requested problem solver not found!");
    }
}
