// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2025, 1, "Secret Entrance")]
public partial class Day1Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var position = 50;
        var clicks = 0;
        foreach (var ins in ParseInput(input))
        {
            position = ins.Direction switch
            {
                Direction.Left => position - ins.Offset,
                Direction.Right => position + ins.Offset,
                _ => throw new InvalidOperationException("Invalid direction")
            };

            while (position < 0 || position > 99)
            {
                if (position > 99)
                {
                    position -= 100;
                }
                else if (position < 0)
                {
                    position += 100;
                }
            }

            if (position == 0)
            {
                clicks++;
            }
        }

        return clicks;
    }

    public object? PartTwo(string input)
    {
        var position = 50;
        var clicks = 0;
        foreach (var (direction, offset) in ParseInput(input))
        {
            for (var i = 1; i <= offset; i++)
            {
                position = direction switch
                {
                    Direction.Left => position - 1,
                    Direction.Right => position + 1,
                    _ => throw new InvalidOperationException("Invalid direction")
                };

                if (position > 99)
                {
                    position = 0;
                }
                else if (position < 0)
                {
                    position = 99;
                }

                if (position == 0)
                {
                    clicks++;
                }
            }
        }

        return clicks;
    }

    private static IEnumerable<Instruction> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            yield return new Instruction((Direction)line.AsSpan(0, 1)[0], int.Parse(line[1..]));
        }
    }

    private record struct Instruction(Direction Direction, int Offset);
}
