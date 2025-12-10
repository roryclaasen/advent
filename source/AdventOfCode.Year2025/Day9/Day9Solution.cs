// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2025, 9, "Movie Theater")]
public partial class Day9Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var redTiles = ParseInput(input);

        var maxArea = 0L;
        for (int i = 0; i < redTiles.Length; i++)
        {
            for (int j = i + 1; j < redTiles.Length; j++)
            {
                var point1 = redTiles[i];
                var point2 = redTiles[j];

                long minX = (long)Math.Min(point1.X, point2.X);
                long maxX = (long)Math.Max(point1.X, point2.X);
                long minY = (long)Math.Min(point1.Y, point2.Y);
                long maxY = (long)Math.Max(point1.Y, point2.Y);

                if (minX == maxX || minY == maxY)
                {
                    continue;
                }

                maxArea = Math.Max(maxArea, (maxX - minX + 1) * (maxY - minY + 1));
            }
        }

        return maxArea;
    }

    public object? PartTwo(string input)
    {
        // This is hard
        throw new NotImplementedException();
    }

    private static ImmutableArray<Vector2> ParseInput(string input)
    {
        var redTiles = ImmutableArray.CreateBuilder<Vector2>();
        foreach (var line in input.EnumerateLines())
        {
            redTiles.Add(line.ToVector2());
        }

        return redTiles.ToImmutable();
    }
}
