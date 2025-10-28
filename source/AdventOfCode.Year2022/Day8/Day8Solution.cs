// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2022;

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2022, 8, "Treetop Tree House")]
public partial class Day8Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var parsedInput = ParseInput(input);
        List<Cell> VisibleTrees(int x, int y, Direction direction)
        {
            var trees = new List<Cell>();

            var line = GetInputLine(parsedInput, x, y, direction);
            var start = line[0];

            var tallest = start;
            for (var i = 1; i < line.Count; i++)
            {
                var cell = line[i];

                if (cell.Value > tallest.Value)
                {
                    tallest = cell;
                    trees.Add(cell);
                }
            }

            trees.Add(start);

            return trees;
        }

        var visibleTrees = new List<Cell>();

        for (var x = 0; x < parsedInput.Width; x++)
        {
            visibleTrees.AddRange(VisibleTrees(x, 0, Direction.Down));
            visibleTrees.AddRange(VisibleTrees(x, parsedInput.Height - 1, Direction.Up));
        }

        for (var y = 0; y < parsedInput.Height; y++)
        {
            visibleTrees.AddRange(VisibleTrees(0, y, Direction.Right));
            visibleTrees.AddRange(VisibleTrees(parsedInput.Width - 1, y, Direction.Left));
        }

        return visibleTrees.Distinct().Count();
    }

    public object? PartTwo(string input)
    {
        var parsedInput = ParseInput(input);
        int VisibleTrees(int x, int y, Direction direction)
        {
            var trees = new List<Cell>();

            var line = GetInputLine(parsedInput, x, y, direction);
            var start = line[0];

            var tallest = start;
            for (var i = 1; i < line.Count; i++)
            {
                var cell = line[i];

                if (cell.Value >= start.Value)
                {
                    tallest = cell;
                    trees.Add(cell);
                    break;
                }

                if (cell.Value > tallest.Value)
                {
                    tallest = cell;
                    trees.Add(cell);
                }
                else if (tallest == start && cell.Value < start.Value)
                {
                    trees.Add(cell);
                }
            }

            return trees.Count;
        }

        var inwardsOffset = 0;
        var scores = new List<int>();
        for (var x = inwardsOffset; x < parsedInput.Width - inwardsOffset; x++)
        {
            for (var y = inwardsOffset; y < parsedInput.Height - inwardsOffset; y++)
            {
                var north = VisibleTrees(x, y, Direction.Up);
                var south = VisibleTrees(x, y, Direction.Down);
                var east = VisibleTrees(x, y, Direction.Right);
                var west = VisibleTrees(x, y, Direction.Left);
                var score = north * south * east * west;
                scores.Add(score);
            }
        }

        return scores.Max();
    }

    private static GridData ParseInput(string input)
    {
        var lines = input.Lines().ToArray();
        var width = lines[0].Length;
        var height = lines.Length;

        Cell[,] grid = new Cell[width, height];

        for (var y = 0; y < height; y++)
        {
            var row = lines[y].ToArray().Select(s => s.ToString()).Select(int.Parse).ToArray();
            for (var x = 0; x < width; x++)
            {
                grid[x, y] = new Cell(x, y, row[x]);
            }
        }

        return new GridData(width, height, grid);
    }

    private static List<Cell> GetInputLine(GridData grid, int x, int y, Direction direction)
    {
        var results = new List<Cell>();

        int xStep = 0, yStep = 0;
        if (direction == Direction.Up || direction == Direction.Down)
        {
            yStep = direction == Direction.Up ? -1 : 1;
        }
        else
        {
            xStep = direction == Direction.Right ? 1 : -1;
        }

        for (var i = 0; i < Math.Max(grid.Width, grid.Height); i++)
        {
            var cX = x + (xStep * i);
            var cY = y + (yStep * i);

            if (cX < 0 || cX >= grid.Width || cY < 0 || cY >= grid.Height)
            {
                break;
            }

            results.Add(grid.Grid[cX, cY]);
        }

        return results;
    }

    private record GridData(int Width, int Height, Cell[,] Grid);

    private record Cell(int X, int Y, int Value);
}
