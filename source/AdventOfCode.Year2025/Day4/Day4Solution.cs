// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Extensions;
using CommunityToolkit.HighPerformance;

[Problem(2025, 4, "Printing Department")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input) => PickUpRollsOfPaper(ParseInput(input));

    public object? PartTwo(string input)
    {
        var map = ParseInput(input);
        var collectedPaper = 0;
        int lastCollected;
        do
        {
            lastCollected = PickUpRollsOfPaper(map, removeOnceCollected: true);
            collectedPaper += lastCollected;
        } while (lastCollected >= 1);

        return collectedPaper;
    }

    private static int PickUpRollsOfPaper(Span2D<bool> map, bool removeOnceCollected = false)
    {
        var collectedPaper = 0;
        var width = map.Width;
        var height = map.Height;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (!map[y, x])
                {
                    continue;
                }

                var currentCount = 0;
                var position = new Vector2(x, y);
                foreach (var p in position.EightNeighbors().Where(p => p.WithinBounds(width, height)))
                {
                    if (map[(int)p.Y, (int)p.X])
                    {
                        currentCount++;
                    }
                }

                if (currentCount < 4)
                {
                    collectedPaper++;
                    if (removeOnceCollected)
                    {
                        map[y, x] = false;
                    }
                }
            }
        }

        return collectedPaper;
    }

    private static Span2D<bool> ParseInput(string input) => input.Lines().ToMatrixBool('@');
}
