// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Buffers;
using System.Collections.Generic;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using CommunityToolkit.HighPerformance;

[Problem(2025, 7, "Laboratories")]
public partial class Day7Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var totalSplits = 0;
        var sourceSearch = SearchValues.Create(['S', '|']);

        var grid = ParseInput(input);
        for (var row = 1; row < grid.Height; row++)
        {
            var sourceSpan = grid.GetRowSpan(row - 1);
            var targetSpan = grid.GetRowSpan(row);

            var splitLocations = new HashSet<int>();
            for (var col = 0; col < grid.Width; col++)
            {
                if (sourceSearch.Contains(sourceSpan[col]))
                {
                    if (targetSpan[col] == '.')
                    {
                        targetSpan[col] = '|';
                    }
                    else if (targetSpan[col] == '^')
                    {
                        splitLocations.Add(col);

                        if (col > 0)
                        {
                            targetSpan[col - 1] = '|';
                        }

                        if (col < grid.Width - 1)
                        {
                            targetSpan[col + 1] = '|';
                        }
                    }
                }
            }

            totalSplits += splitLocations.Count;
        }

        return totalSplits;

        static Span2D<char> ParseInput(string input)
        {
            var readOnlySpan2d = input.GetWidthAndHeight(out var width, out var height);
            Span<char> spanCopy = new char[readOnlySpan2d.Length];
            readOnlySpan2d.CopyTo(spanCopy);
            return spanCopy.AsSpan2D(height, width);
        }
    }

    public object? PartTwo(string input)
    {
        var grid = input.AsSpan2D();
        var timelineGrid = new Span2D<long>(Array.Create((int)grid.Length, 1L), grid.Height, grid.Width);

        for (var row = grid.Height - 2; row >= 0; row--)
        {
            var rowSpan = grid.GetRowSpan(row);
            for (var col = 0; col < grid.Width; col++)
            {
                if (rowSpan[col] == '^')
                {
                    var totalTimelines = 0L;
                    if (col > 0)
                    {
                        totalTimelines += timelineGrid[row + 1, col - 1];
                    }

                    if (col < grid.Width - 1)
                    {
                        totalTimelines += timelineGrid[row + 1, col + 1];
                    }

                    timelineGrid[row, col] = totalTimelines;
                }
                else
                {
                    timelineGrid[row, col] = timelineGrid[row + 1, col];
                }
            }
        }

        return timelineGrid[0, grid.GetRowSpan(0).IndexOf('S')];
    }
}
