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
        var returnedList = new List<TType>(input.Count);
        returnedList.AddRange(input);

        return returnedList;
    }

    public static TType[] ShallowClone<TType>(this TType[] input)
    {
        var returnedArray = new TType[input.Length];

        for (int ii = 0; ii<input.Length; ii++)
        {
            returnedArray[ii] = input[ii];
        }

        return returnedArray;
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

    public static void AddInstance<TKey, TValue>(this SortedList<TKey, List<TValue>> dict, TKey category, TValue valueToAdd)
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

    public static void AddToCell(this Dictionary<long, long> dict, long cell, long amount)
    {
        if (dict.TryGetValue(cell, out var value))
        {
            dict[cell] = value + amount;
        }
        else
        {
            dict.Add(cell, amount);
        }
    }
}
