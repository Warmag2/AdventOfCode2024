namespace AdventOfCode2024.Extensions;

public static class ListExtensions
{
    public static void Swap<TType>(this List<TType> input, int idx1, int idx2)
    {
        var temp = input[idx1];
        input[idx1] = input[idx2];
        input[idx2] = temp;
    }

    public static List<TType> ShallowClone<TType>(this List<TType> input)
    {
        var returnedList = new List<TType>();
        returnedList.AddRange(input);

        return returnedList;
    }
}
