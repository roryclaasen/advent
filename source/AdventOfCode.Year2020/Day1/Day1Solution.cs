namespace AdventOfCode.Year2020;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2020, 1, "Report Repair")]
public class Day1Solution : ISolver
{
    private static readonly int Target = 2020;

    public object? PartOne(string input)
    {
        var entries = ParseInput(input);
        return ProductEntriesThatSum(Target, 2, entries.ToArray());
    }

    public object? PartTwo(string input)
    {
        var entries = ParseInput(input);
        return ProductEntriesThatSum(Target, 3, entries.ToArray());
    }

    static IEnumerable<int> ParseInput(string input)
        => input.SplitNewLine().Select(int.Parse);

    static int ProductEntriesThatSum(int target, int count, params int[] entries)
    {
        var combinations = Combinations(entries, count);
        var matchingCombination = combinations.FirstOrDefault(c => c.Sum() == target);
        return matchingCombination?.Aggregate(1, (a, b) => a * b) ?? 0;
    }

    static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> elements, int count)
        => count == 0
            ? new[] { Array.Empty<T>() }
            : elements.SelectMany((e, i) =>
                Combinations(elements.Skip(i + 1), count - 1).Select(c => (new[] { e }).Concat(c)));
}
