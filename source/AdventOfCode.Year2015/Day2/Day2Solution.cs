namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System;
using System.Linq;

[Problem(2015, 2, "I Was Told There Would Be No Math")]
public class Day2Solution : ISolver
{
    public object? PartOne(string input)
    {
        var parsed = this.ParseInput(input);
        var total = 0;
        foreach (var box in parsed)
        {
            var sides = new List<int>
            {
                box.Length * box.Width,
                box.Width * box.Height,
                box.Height * box.Length,
            };

            total += (sides.Sum() * 2) + sides.Min();
        }

        return total;
    }

    public object? PartTwo(string input)
    {
        var parsed = this.ParseInput(input);
        var total = 0;
        foreach (var box in parsed)
        {
            var measurements = new List<int>
            {
                box.Length,
                box.Width,
                box.Height,
            };
            measurements.Sort();

            total += (measurements.Take(2).Sum() * 2) + (box.Length * box.Width * box.Height);
        }

        return total;
    }

    private IEnumerable<Dimension> ParseInput(string input)
        => input
            .SplitNewLine()
            .Select(line => line.Split('x').Select(int.Parse))
            .Select(line =>
            {
                var array = line.ToArray();
                return new Dimension(array[0], array[1], array[2]);
            });

    private record Dimension(int Length, int Width, int Height);
}
