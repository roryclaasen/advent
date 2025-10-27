// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2020;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2020, 1, "Report Repair")]
public partial class Day1Solution : IProblemSolver
{
    private static readonly int Target = 2020;

    public object? PartOne(string input)
    {
        var entries = ParseInput(input);
        return ProductEntriesThatSum(Target, 2, [.. entries]);
    }

    public object? PartTwo(string input)
    {
        var entries = ParseInput(input);
        return ProductEntriesThatSum(Target, 3, [.. entries]);
    }

    private static IEnumerable<int> ParseInput(string input)
        => input.Lines().Select(int.Parse);

    private static int ProductEntriesThatSum(int target, int count, params int[] entries)
    {
        var matchingCombination = entries.Combinations(count).Single(c => c.Sum() == target);
        return matchingCombination.Product();
    }
}
