namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System;
using System.Linq;

[Problem(2022, 1, "Calorie Counting")]
public class Day1Solution : ISolver
{
    public object? PartOne(string input)
    {
        var elves = this.ParseInput(input);
        var max = elves.MaxBy(e => e.Calories);
        return max?.Calories;
    }

    public object? PartTwo(string input)
    {
        var elves = this.ParseInput(input);
        return elves
            .Select(e => e.Calories)
            .Order()
            .TakeLast(3)
            .Sum();
    }

    private IEnumerable<Elf> ParseInput(string input)
    {
        foreach (var section in input.Split(Environment.NewLine + Environment.NewLine))
        {
            yield return new Elf(section.Lines().Select(int.Parse).Sum());
        }
    }

    private record Elf(int Calories);
}
