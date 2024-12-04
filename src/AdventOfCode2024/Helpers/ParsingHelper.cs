namespace AdventOfCode2024.Helpers;

public static class ParsingHelper
{
    public static char[][] ToArray2D(this string input, char[] delimeterSequence)
    {
        return input.ToArray().ToArray2D(delimeterSequence);
    }

    public static TType[][] ToArray2D<TType>(this IList<TType> input, TType[] delimeterSequence)
        where TType : IEquatable<TType>
    {
        var returnValue = new List<TType[]>();

        int cursor = 0;

        while (cursor < input.Count)
        {
            returnValue.Add(ExtractUntilDelimeter(input, delimeterSequence, cursor));
            cursor += returnValue[^1].Length;
            cursor += delimeterSequence.Length;
        }

        return returnValue.ToArray();
    }

    private static int FindNextSequenceIndex<TType>(IList<TType> input, TType[] delimeterSequence, int cursor = 0)
        where TType : IEquatable<TType>
    {
        while (cursor < input.Count)
        {
            if (input[cursor].Equals(delimeterSequence[0]))
            {
                var found = true;

                for (int ii = 1; ii < delimeterSequence.Length; ii++)
                {
                    if (!input[cursor + ii].Equals(delimeterSequence[ii]))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return cursor;
                }
            }

            cursor++;
        }

        return input.Count;
    }

    private static TType[] ExtractUntilDelimeter<TType>(IList<TType> input, TType[] delimeterSequence, int cursor = 0)
        where TType : IEquatable<TType>
    {
        return input.Skip(cursor).Take(FindNextSequenceIndex(input, delimeterSequence, cursor) - cursor).ToArray();
    }
}
