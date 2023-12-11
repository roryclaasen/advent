namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System.Linq;
using System;

[Problem(2022, 6, "Tuning Trouble")]
public class Day6Solution : ISolver
{
    public object? PartOne(string input) => FindMarker(input, 4);

    public object? PartTwo(string input) => FindMarker(input, 14);

    private static int FindMarker(string input, int length)
    {
        for (var i = 0; i < input.Length - length - 1; i++)
        {
            var marker = input.Substring(i, length);
            if (marker.Distinct().Count() == length)
            {
                return i + length;
            }
        }

        throw new Exception("Unbale to find marker");
    }
}
