namespace AdventOfCode.Year2024;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2024, 2, "Red-Nosed Reports")]
public class Day2Solution : ISolver
{
    public object? PartOne(string input)
        => ParseInput(input).Count(AreLevelsSafe);

    public object? PartTwo(string input)
    {
        static bool PartTwoSafe(IEnumerable<int> report)
        {
            if (AreLevelsSafe(report))
            {
                return true;
            }

            for (var i = 0; i < report.Count(); i++)
            {
                var newReport = new List<int>(report);
                newReport.RemoveAt(i);
                if (AreLevelsSafe(newReport))
                {
                    return true;
                }
            }

            return false;
        }

        return ParseInput(input).Count(PartTwoSafe);
    }

    private static IEnumerable<IEnumerable<int>> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            yield return line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        }
    }

    static bool AreLevelsSafe(IEnumerable<int> report)
    {
        var first = report.First();
        var second = report.Skip(1).First();

        var firstDiff = Math.Abs(first - second);
        if (firstDiff < 1 || firstDiff > 3)
        {
            return false;
        }

        var direction = second.CompareTo(first);

        var last = second;
        foreach (var level in report.Skip(2))
        {
            if (level.CompareTo(last) != direction)
            {
                return false;
            }

            var diff = Math.Abs(level - last);
            if (diff < 1 || diff > 3)
            {
                return false;
            }

            last = level;
        }

        return true;
    }
}
