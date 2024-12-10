namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using CommunityToolkit.HighPerformance;
using System.Collections.Generic;
using System.Numerics;

[Problem(2024, 10, "Hoof It")]
public partial class Day10Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var map = ParseInput(input);

        var total = 0;
        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                if (map[y, x] != 0)
                {
                    continue;
                }

                total += GetAllTrailHeads(map, 0, x, y).Count;
            }
        }

        return total;
    }

    public object? PartTwo(string input)
    {
        var map = ParseInput(input);

        var total = 0;
        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                if (map[y, x] != 0)
                {
                    continue;
                }

                total += GetAllDistinctTrailHeads(map, 0, x, y);
            }
        }

        return total;
    }

    private static HashSet<Vector2> GetAllTrailHeads(ReadOnlySpan2D<int> map, int trailHeadToFind, int x, int y)
    {
        if (trailHeadToFind != map[y, x])
        {
            return [];
        }

        if (trailHeadToFind == 9)
        {
            return [new Vector2(x, y)];
        }

        var trailHeads = new HashSet<Vector2>();
        var next = trailHeadToFind + 1;
        if ((x + 1) < map.Width)
        {
            trailHeads.AddRange(GetAllTrailHeads(map, next, x + 1, y));
        }
        if ((x - 1) >= 0)
        {
            trailHeads.AddRange(GetAllTrailHeads(map, next, x - 1, y));
        }
        if ((y + 1) < map.Height)
        {
            trailHeads.AddRange(GetAllTrailHeads(map, next, x, y + 1));
        }
        if ((y - 1) >= 0)
        {
            trailHeads.AddRange(GetAllTrailHeads(map, next, x, y - 1));
        }

        return trailHeads;
    }

    private static int GetAllDistinctTrailHeads(ReadOnlySpan2D<int> map, int trailHeadToFind, int x, int y)
    {
        if (trailHeadToFind != map[y, x])
        {
            return 0;
        }

        if (trailHeadToFind == 9)
        {
            return 1;
        }

        var trailHeads = 0;
        var next = trailHeadToFind + 1;
        if ((x + 1) < map.Width)
        {
            trailHeads += GetAllDistinctTrailHeads(map, next, x + 1, y);
        }
        if ((x - 1) >= 0)
        {
            trailHeads += GetAllDistinctTrailHeads(map, next, x - 1, y);
        }
        if ((y + 1) < map.Height)
        {
            trailHeads += GetAllDistinctTrailHeads(map, next, x, y + 1);
        }
        if ((y - 1) >= 0)
        {
            trailHeads += GetAllDistinctTrailHeads(map, next, x, y - 1);
        }

        return trailHeads;
    }

    private static ReadOnlySpan2D<int> ParseInput(string input) => input.Lines().ToMatrixInt();
}
