namespace AdventOfCode.Year2018;

using AdventOfCode.Shared;
using System.Linq;

[Problem(2018, 2, "Inventory Management System")]
public partial class Day2Solution : ISolver
{
    public object? PartOne(string input)
    {
        var twos = 0;
        var threes = 0;

        foreach(var line in input.Lines())
        {
            var counts = line.GroupBy(c => c).Select(g => g.Count()).ToArray();
            if (counts.Contains(2))
            {
                twos++;
            }
            if (counts.Contains(3))
            {
                threes++;
            }
        }

        return twos * threes;
    }

    public object? PartTwo(string input)
    {
        var lines = input.Lines();
        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = i + 1; j < lines.Length; j++)
            {
                var common = lines[i]
                    .Zip(lines[j], (a, b) => a == b ? a : '\0')
                    .Where(c => c != '\0')
                    .ToArray();
                if (common.Length == lines[i].Length - 1)
                {
                    return new string(common);
                }
            }
        }

        return null;
    }
}
