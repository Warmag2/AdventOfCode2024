namespace AdventOfCode2024.Problems;

public abstract class ProblemBase : IProblem
{
    protected readonly string Input;

    protected ProblemBase(string input)
    {
        Input = input;
    }

    public abstract string Solve();
}
