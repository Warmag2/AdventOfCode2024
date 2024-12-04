namespace AdventOfCode2024.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ProblemIndexAttribute : Attribute
{
    public int Index { get; set; }
}
