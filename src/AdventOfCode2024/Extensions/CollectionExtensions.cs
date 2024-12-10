namespace AdventOfCode2024.Extensions;

public static class CollectionExtensions
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

    public static void AddInstance<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey category, TValue valueToAdd)
        where TKey : struct
    {
        if (dict.TryGetValue(category, out var value))
        {
            value.Add(valueToAdd);
        }
        else
        {
            dict.Add(category, new List<TValue>() { valueToAdd });
        }
    }
}
