namespace AdventOfCode.Year2024;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2024, 1, "Historian Hysteria")]
public partial class Day1Solution : ISolver
{
    public object? PartOne()
    {
        var (Left, Right) = ParseInput();
        return Left.WithIndex().Sum(i => Math.Abs(Right[i.Index] - i.Item));
    }

    public object? PartTwo()
    {
        var (Left, Right) = ParseInput();
        return Left.Sum(n => n * Right.Count(x => x == n));
    }

    private static (List<int> Left, List<int> Right) ParseInput()
    {
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in input.Lines())
        {
            (var leftNumber, var rightNumber) = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            left.Add(leftNumber);
            right.Add(rightNumber);
        }
        left.Sort();
        right.Sort();
        return (left, right);
    }
}
