// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Extensions;
using LinkDotNet.StringBuilder;

[Problem(2025, 6, "Trash Compactor")]
public partial class Day6Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        long sum = 0;
        foreach (var column in input.VerticalSplit(' '))
        {
            var numbers = new List<long>();
            foreach (var line in column.EnumerateLines())
            {
                var firstChar = line[0];
                if (firstChar == '+' || firstChar == '*')
                {
                    sum += Execute(numbers, firstChar);
                    break;
                }

                if (int.TryParse(line, out var number))
                {
                    numbers.Add(number);
                }
            }
        }

        return sum;
    }

    public object? PartTwo(string input)
    {
        long sum = 0;
        foreach (var column in input.VerticalSplit(' '))
        {
            var span = column.AsSpan2D();
            var numbers = new List<long>();
            for (var col = 0; col < span.Width; col++)
            {
                var num = new ValueStringBuilder(4);
                for (var row = 0; row < span.Height - 1; row++)
                {
                    var character = span[row, col];
                    num.Append(character);
                }

                if (int.TryParse(num.AsSpan(), out var number))
                {
                    numbers.Add(number);
                }
            }

            sum += Execute(numbers, span[span.Height - 1, 0]);
        }

        return sum;
    }

    private static long Execute(IEnumerable<long> number, char instruction) => instruction switch
    {
        '+' => number.Sum(),
        '*' => number.Product(),
        _ => throw new Exception($"Unknown operation: {instruction}"),
    };
}
