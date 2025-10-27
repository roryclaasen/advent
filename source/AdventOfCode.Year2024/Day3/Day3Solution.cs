// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using System;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2024, 3, "Mull It Over")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input)
        => SequenceRegex()
            .Matches(input)
            .Where(m => m.Groups[0].Value.StartsWith("mul", StringComparison.Ordinal))
            .Select(m => (int.Parse(m.Groups[1].ValueSpan), int.Parse(m.Groups[2].ValueSpan)))
            .Sum(m => m.Item1 * m.Item2);

    public object? PartTwo(string input)
    {
        var enabled = true;
        var total = 0;
        foreach (var match in SequenceRegex().Matches(input).Select(s => s))
        {
            if (match.Groups[0].Value.Equals("do()", StringComparison.Ordinal))
            {
                enabled = true;
            }
            else if (match.Groups[0].Value.Equals("don't()", StringComparison.Ordinal))
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
