namespace AdventOfCode.Year2020;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2020, 6, "Custom Customs")]
public partial class Day6Solution : ISolver
{
    public object? PartOne(string input)
    {
        var groups = input.Lines(2).Select(x => x.Replace(Environment.NewLine, string.Empty));
        var answers = groups.Select(x => x.Distinct().Count());
        return answers.Sum();
    }

    public object? PartTwo(string input)
    {
        var groups = input.Lines(2).Select(x => x.Lines());
        var answers = groups.Select(x => x.SelectMany(y => y).GroupBy(y => y).Where(y => y.Count() == x.Length).Count());
        return answers.Sum();
    }
}
