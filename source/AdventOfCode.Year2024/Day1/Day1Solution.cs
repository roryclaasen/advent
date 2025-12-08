// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2024, 1, "Historian Hysteria")]
public partial class Day1Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var (left, right) = ParseInput(input);
        return left.WithIndex().Sum(i => Math.Abs(right[i.Index] - i.Item));
    }

    public object? PartTwo(string input)
    {
        var (left, right) = ParseInput(input);
        return left.Sum(n => n * right.Count(x => x == n));
    }

    private static (List<int> Left, List<int> Right) ParseInput(string input)
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
