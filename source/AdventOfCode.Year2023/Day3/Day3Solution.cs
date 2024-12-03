namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

[Problem(2023, 3, "Gear Ratios")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var sum = 0;
        var grid = input.Lines().ToArray();

        foreach (var (row, rowIndex) in grid.WithIndex())
        {
            foreach (var match in NumberRegex().EnumerateMatches(row))
            {
                for (int i = 0; i < match.Length; i++)
                {
                    var coord = new Vector2(match.Index + i, rowIndex);
                    if (coord
                        .EightNeighbors()
                        .Select(v => GetTile(grid, v))
                        .Any(c => !char.IsDigit(c) && c != '.'))
                    {
                        sum += int.Parse(row.AsSpan().Slice(match.Index, match.Length));
                        break;
                    }
                }
            }
        }
        return sum;
    }

    public object? PartTwo(string input)
    {
        var grid = input.Lines().ToArray();

        Dictionary<Vector2, (Vector2 Start, int Value)> gridNumbers = [];
        foreach (var (row, rowIndex) in grid.WithIndex())
        {
            foreach (var match in NumberRegex().EnumerateMatches(row))
            {
                var start = new Vector2(match.Index, rowIndex);
                for (int i = 0; i < match.Length; i++)
                {
                    var coord = new Vector2(match.Index + i, rowIndex);
                    var value = int.Parse(row.AsSpan().Slice(match.Index, match.Length));
                    gridNumbers.Add(coord, (start, value));
                }
            }
        }

        var sum = 0;
        foreach (var (row, rowIndex) in grid.WithIndex())
        {
            foreach (var match in GearRegex().EnumerateMatches(row))
            {
                var matchVector = new Vector2(match.Index, rowIndex);
                var adjacentNumbers = matchVector
                    .EightNeighbors()
                    .Where(gridNumbers.ContainsKey)
                    .Select(v => gridNumbers[v])
                    .Distinct()
                    .ToArray();
                if (adjacentNumbers.Length == 2)
                {
                    sum += adjacentNumbers[0].Value * adjacentNumbers[1].Value;
                }
            }
        }
        return sum;
    }

    private static char GetTile(string[] grid, Vector2 position)
    {
        if (position.Y < 0 || position.Y >= grid.Length)
        {
            return '.';
        }

        if (position.X < 0 || position.X >= grid[(int)position.Y].Length)
        {
            return '.';
        }

        return grid[(int)position.Y][(int)position.X];
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"\*")]
    private static partial Regex GearRegex();
}
