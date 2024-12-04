namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using CommunityToolkit.HighPerformance;
using System;
using System.Linq;
using System.Text;

[Problem(2024, 4, "Ceres Search")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        ReadOnlySpan<char> XMAS = "XMAS";
        int[][] directions = [[1, 0], [-1, 0], [0, 1], [0, -1], [1, 1], [1, -1], [-1, 1], [-1, -1]];

        var grid = GetWordSearch(input);

        var total = 0;
        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                foreach (var dir in directions)
                {
                    if (CheckWordMatch(XMAS, grid, x, y, dir[1], dir[0]))
                    {
                        total++;
                    }
                }
            }
        }

        return total;
    }

    public object? PartTwo(string input)
    {
        ReadOnlySpan<char> X_MAS = "MAS";
        int[][] directions = [[1, 1], [1, -1], [-1, 1], [-1, -1]];

        var grid = GetWordSearch(input);
        var total = 0;
        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                var matched = 0;
                foreach (var dir in directions)
                {
                    if (CheckWordMatch(X_MAS, grid, x - dir[1], y - dir[0], dir[1], dir[0]))
                    {
                        matched++;
                    }
                }

                if (matched >= 2)
                {
                    total++;
                }
            }
        }

        return total;
    }

    private static bool CheckWordMatch(ReadOnlySpan<char> word, ReadOnlySpan2D<char> grid, int x, int y, int dX, int dY)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < word.Length; i++)
        {
            if (y < 0 || y >= grid.Height || x < 0 || x >= grid.Width)
            {
                break;
            }

            sb.Append(grid[y, x]);
            x += dX;
            y += dY;
        }

        return sb.Equals(word);
    }

    private static ReadOnlySpan2D<char> GetWordSearch(string input)
        => input.Lines().Select(l => l.ToUpperInvariant().ToCharArray()).ToArray().ToMatrix();
}
