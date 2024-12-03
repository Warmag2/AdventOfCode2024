﻿namespace AdventOfCode2024.Extensions;

public static class ListExtensions
{
    public static List<TType> ShallowClone<TType>(this List<TType> input)
    {
        var returnedList = new List<TType>();
        returnedList.AddRange(input);

        return returnedList;
    }
}