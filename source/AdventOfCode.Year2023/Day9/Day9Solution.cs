namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2023, 9, "Mirage Maintenance")]
public partial class Day9Solution : ISolver
{
    public object? PartOne(string input)
    {
        var total = 0;
        foreach (var numbers in ParseInput(input))
        {
            var diffs = GetDiffs(numbers);

            for (var i = diffs.Count - 1; i > 0; i--)
            {
                diffs[i - 1].Add(diffs[i - 1][^1] + diffs[i][^1]);
            }

            total += diffs[0][^1];
        }

        return total;
    }

    public object? PartTwo(string input)
    {
        var total = 0;
        foreach (var numbers in ParseInput(input))
        {
            var diffs = GetDiffs(numbers);

            for (var i = diffs.Count - 1; i > 0; i--)
            {
                diffs[i - 1].Insert(0, diffs[i - 1][0] - diffs[i][0]);
            }

            total += diffs[0][0];
        }

        return total;
    }

    private static List<List<int>> GetDiffs(List<int> input)
    {
        var diffs = new List<List<int>> { input };

        var current = new List<int>(input);
        while (current.Any(x => x != 0))
        {
            current = [];

            for (var i = 0; i < diffs[^1].Count - 1; i++)
            {
                var diff = diffs[^1][i + 1] - diffs[^1][i];
                current.Add(diff);
            }

            diffs.Add(current);
        }

        return diffs;
    }

    private static IEnumerable<List<int>> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            yield return line.Split(' ').Select(int.Parse).ToList();
        }
    }
}
