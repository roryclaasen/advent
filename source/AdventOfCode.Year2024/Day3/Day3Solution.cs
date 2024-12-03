namespace AdventOfCode.Year2024;

using AdventOfCode.Shared;
using System.Text.RegularExpressions;
using System.Linq;

[Problem(2024, 3, "Mull It Over")]
public partial class Day3Solution : ISolver
{
    public object? PartOne()
        => SequenceRegex()
            .Matches(input)
            .Where(m => m.Groups[0].Value.StartsWith("mul"))
            .Select(m => (int.Parse(m.Groups[1].ValueSpan), int.Parse(m.Groups[2].ValueSpan)))
            .Sum(m => m.Item1 * m.Item2);

    public object? PartTwo()
    {
        var enabled = true;
        var total = 0;
        foreach (var match in SequenceRegex().Matches(input).Select(s => s))
        {
            if (match.Groups[0].Value.Equals("do()"))
            {
                enabled = true;
            }
            else if (match.Groups[0].Value.Equals("don't()"))
            {
                enabled = false;
            }
            else if (enabled)
            {
                total += int.Parse(match.Groups[1].ValueSpan) * int.Parse(match.Groups[2].ValueSpan);
            }
        }

        return total;
    }

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d+),(\d+)\)")]
    public static partial Regex SequenceRegex();
}
