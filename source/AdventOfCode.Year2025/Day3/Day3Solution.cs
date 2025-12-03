// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using AdventOfCode.Problem;

[Problem(2025, 3, "Lobby")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input) => GetJoltage(input, 2);

    public object? PartTwo(string input) => GetJoltage(input, 12);

    private static long GetJoltage(string input, int length)
    {
        var joltage = 0L;
        foreach (var line in input.EnumerateLines())
        {
            joltage += GetJoltagePerBank(line, length);
        }

        return joltage;
    }

    private static long GetJoltagePerBank(ReadOnlySpan<char> line, int length)
    {
        var joltage = new char[length];
        var lastIndex = -1;

        for (var i = 0; i < length; i++)
        {
            var start = lastIndex + 1;
            var end = line.Length - (length - i) + 1;

            (joltage[i], lastIndex) = GetMaxFirstCharacter(line[start..end]);
            lastIndex += start;
        }

        return long.Parse(joltage);
    }

    private static (char Character, int Position) GetMaxFirstCharacter(ReadOnlySpan<char> line)
    {
        var current = line[0];
        var position = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] > current)
            {
                current = line[i];
                position = i;
            }
        }

        return (current, position);
    }
}
