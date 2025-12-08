// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;

[Problem(2025, 2, "Gift Shop")]
public partial class Day2Solution : IProblemSolver
{
    public object? PartOne(string input) => Implementation(input, CountPart1);

    public object? PartTwo(string input) => Implementation(input, CountPart2);

    private static long Implementation(string input, Func<long, long> countFunc)
    {
        long count = 0;
        foreach (var (start, end) in ParseInput(input))
        {
            for (long current = start; current <= end; current++)
            {
                count += countFunc.Invoke(current);
            }
        }

        return count;
    }

    private static long CountPart1(long number)
    {
        var numberStr = number.ToString();
        if (numberStr.Length % 2 != 0)
        {
            return 0;
        }

        var halfStr = numberStr.AsSpan(0, numberStr.Length / 2);
        return numberStr.EndsWith(halfStr) ? number : 0;
    }

    private static long CountPart2(long number)
    {
        var numberStr = number.ToString();
        var halfLength = numberStr.Length / 2;

        for (var length = 1; length <= halfLength; length++)
        {
            if (numberStr.Length % length != 0)
            {
                continue;
            }

            var pattern = numberStr.AsSpan(0, length);
            bool isRepeating = true;

            for (int i = length; i < numberStr.Length; i += length)
            {
                if (!numberStr.AsSpan(i, length).SequenceEqual(pattern))
                {
                    isRepeating = false;
                    break;
                }
            }

            if (isRepeating)
            {
                return number;
            }
        }

        return 0;
    }

    private static IEnumerable<(long Start, long End)> ParseInput(string input)
    {
        foreach (var line in input.Split(','))
        {
            var parts = line.Split('-');
            yield return (long.Parse(parts[0]), long.Parse(parts[1]));
        }
    }
}
