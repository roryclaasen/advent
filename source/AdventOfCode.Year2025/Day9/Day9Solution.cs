// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        var redTiles = ParseInput(input);
        var edges = BuildEdges(redTiles);

        var maxArea = 0L;

        // Try all pairs of red tiles as rectangle corners
        for (int i = 0; i < redTiles.Length; i++)
        {
            for (int j = i + 1; j < redTiles.Length; j++)
            {
                var p1 = redTiles[i];
                var p2 = redTiles[j];

                long minX = (long)Math.Min(p1.X, p2.X);
                long maxX = (long)Math.Max(p1.X, p2.X);
                long minY = (long)Math.Min(p1.Y, p2.Y);
                long maxY = (long)Math.Max(p1.Y, p2.Y);

                if (minX == maxX || minY == maxY)
                {
                    continue;
                }

                long area = (maxX - minX + 1) * (maxY - minY + 1);

                // Skip if can't beat current max
                if (area <= maxArea)
                {
                    continue;
                }

                // Check if rectangle is fully inside polygon
                if (IsRectangleInsidePolygon(minX, maxX, minY, maxY, edges, redTiles))
                {
                    maxArea = area;
                }
            }
        }

        return maxArea;
    }

    private static List<Edge> BuildEdges(ImmutableArray<Vector2> redTiles)
    {
        var edges = new List<Edge>(redTiles.Length);

        for (int i = 0; i < redTiles.Length; i++)
        {
            var current = redTiles[i];
            var next = redTiles[(i + 1) % redTiles.Length];
            edges.Add(new Edge(current, next));
        }

        return edges;
    }

    private static bool IsRectangleInsidePolygon(long minX, long maxX, long minY, long maxY, IEnumerable<Edge> edges, ImmutableArray<Vector2> redTiles)
    {
        if (!IsPointInsideOrOnPolygon(minX, minY, edges, redTiles) ||
            !IsPointInsideOrOnPolygon(maxX, minY, edges, redTiles) ||
            !IsPointInsideOrOnPolygon(minX, maxY, edges, redTiles) ||
            !IsPointInsideOrOnPolygon(maxX, maxY, edges, redTiles))
        {
            return false;
        }

        foreach (var edge in edges)
        {
            if (EdgeCrossesRectangleInterior(edge, minX, maxX, minY, maxY))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsPointInsideOrOnPolygon(long px, long py, IEnumerable<Edge> edges, ImmutableArray<Vector2> redTiles)
    {
        foreach (var tile in redTiles)
        {
            if ((long)tile.X == px && (long)tile.Y == py)
            {
                return true;
            }
        }

        foreach (var edge in edges)
        {
            if (IsPointOnEdge(px, py, edge))
            {
                return true;
            }
        }

        int crossings = 0;
        foreach (var edge in edges)
        {
            if (RayCrossesEdge(px, py, edge))
            {
                crossings++;
            }
        }

        return crossings % 2 == 1;
    }

    private static bool IsPointOnEdge(long px, long py, Edge edge)
    {
        long x1 = (long)edge.Start.X;
        long y1 = (long)edge.Start.Y;
        long x2 = (long)edge.End.X;
        long y2 = (long)edge.End.Y;

        if (edge.IsHorizontal)
        {
            return py == y1 && px >= Math.Min(x1, x2) && px <= Math.Max(x1, x2);
        }
        else
        {
            return px == x1 && py >= Math.Min(y1, y2) && py <= Math.Max(y1, y2);
        }
    }

    private static bool RayCrossesEdge(long px, long py, Edge edge)
    {
        long x1 = (long)edge.Start.X;
        long y1 = (long)edge.Start.Y;
        long y2 = (long)edge.End.Y;

        if (edge.IsHorizontal)
        {
            return false;
        }

        long edgeX = x1;
        long minY = Math.Min(y1, y2);
        long maxY = Math.Max(y1, y2);

        if (edgeX <= px || py < minY || py >= maxY)
        {
            return false;
        }

        return true;
    }

    private static bool EdgeCrossesRectangleInterior(Edge edge, long minX, long maxX, long minY, long maxY)
    {
        long x1 = (long)edge.Start.X, y1 = (long)edge.Start.Y;
        long x2 = (long)edge.End.X, y2 = (long)edge.End.Y;

        if (edge.IsHorizontal)
        {
            long edgeY = y1;
            long edgeMinX = Math.Min(x1, x2);
            long edgeMaxX = Math.Max(x1, x2);

            if (edgeY > minY && edgeY < maxY && edgeMinX < maxX && edgeMaxX > minX)
            {
                return true;
            }
        }
        else
        {
            long edgeX = x1;
            long edgeMinY = Math.Min(y1, y2);
            long edgeMaxY = Math.Max(y1, y2);

            if (edgeX > minX && edgeX < maxX && edgeMinY < maxY && edgeMaxY > minY)
            {
                return true;
            }
        }

        return false;
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

    private readonly record struct Edge(Vector2 Start, Vector2 End)
    {
        public bool IsHorizontal => this.Start.Y == this.End.Y;
    }
}
