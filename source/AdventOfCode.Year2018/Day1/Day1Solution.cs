// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2018;

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2018, 1, "Chronal Calibration")]
public partial class Day1Solution : IProblemSolver
{
    public object? PartOne(string input)
        => ParseInput(input).Sum();

    public object? PartTwo(string input)
    {
        const int maxIterations = 10000;
        var changes = ParseInput(input).ToArray();
        var history = new HashSet<int>() { 0 };
        var frequency = 0;
        for (var i = 0; i < changes.Length * maxIterations; i++)
        {
            var change = changes[i % changes.Length];
            frequency += change;
            if (!history.Add(frequency))
            {
                return frequency;
            }
        }

        throw new Exception($"Unable to find a solution after {maxIterations} iterations");
    }

    private static IEnumerable<int> ParseInput(string input)
        => input.Lines().Select(int.Parse);
}
