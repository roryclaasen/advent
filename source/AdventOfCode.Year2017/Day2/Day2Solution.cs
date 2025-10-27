// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2017;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2017, 2, "Corruption Checksum")]
public partial class Day2Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var sum = 0;
        foreach (var row in ParseInput(input))
        {
            sum += row.Max() - row.Min();
        }

        return sum;
    }

    public object? PartTwo(string input)
    {
        var sum = 0;
        foreach (var row in ParseInput(input))
        {
            for (var i = 0; i < row.Length; i++)
            {
                for (var j = 0; j < row.Length; j++)
                {
                    if (i == j) continue;

                    if (row[i] % row[j] == 0)
                    {
                        sum += row[i] / row[j];
                    }
                }
            }
        }

        return sum;
    }

    static IEnumerable<int[]> ParseInput(string input)
    {
        foreach (var line in input.Replace('\t', ' ').Lines())
        {
            yield return line.Split(' ').Select(int.Parse).ToArray();
        }
    }
}
