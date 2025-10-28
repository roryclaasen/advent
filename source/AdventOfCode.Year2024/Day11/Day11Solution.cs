// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Extensions;

[Problem(2024, 11, "Plutonian Pebbles")]
public partial class Day11Solution : IProblemSolver
{
    private readonly Dictionary<(long, int), long> stoneCache = [];

    public object? PartOne(string input)
        => ParseInput(input).Sum(s => this.Calculate(s, 25));

    public object? PartTwo(string input)
        => ParseInput(input).Sum(s => this.Calculate(s, 75));

    private static IEnumerable<long> ParseInput(string input) => input.Spaces().Select(long.Parse);

    private long Calculate(long stone, int blinks)
    {
        if (blinks == 0)
        {
            return 1;
        }

        var key = (stone, blinks);
        if (this.stoneCache.TryGetValue(key, out var value))
        {
            return value;
        }

        if (stone == 0)
        {
            value = this.Calculate(1, blinks - 1);
        }
        else
        {
            var digits = stone.CountDigits();
            if (digits % 2 != 0)
            {
                value = this.Calculate(stone * 2024, blinks - 1);
            }
            else
            {
                var (left, right) = stone.SplitDigits(digits / 2);
                value = this.Calculate(left, blinks - 1) + this.Calculate(right, blinks - 1);
            }
        }

        this.stoneCache.Add(key, value);
        return value;
    }
}
