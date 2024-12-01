namespace AdventOfCode.Year2024;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2024, 1, "Historian Hysteria")]
public class Day1Solution : ISolver
{
    public object? PartOne(string input)
    {
        var (Left, Right) = ParseInput(input);
        return Left.WithIndex().Sum(i => Math.Abs(Right[i.Index] - i.Item));
    }

    public object? PartTwo(string input)
    {
        var (Left, Right) = ParseInput(input);
        return Left.Sum(n => n * Right.Count(x => x == n));
    }

    private (List<int> Left, List<int> Right) ParseInput(string input)
    {
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in input.Lines())
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(parts[0]));
            right.Add(int.Parse(parts[1]));
        }
        left.Sort();
        right.Sort();
        return (left, right);
    }
}
