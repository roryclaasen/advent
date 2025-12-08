// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Extensions;

[Problem(2025, 8, "Playground")]
public partial class Day8Solution : IProblemSolver
{
    internal int RequiredPairs { get; set; } = 1000;

    public object? PartOne(string input)
    {
        var boxes = ParseInput(input);
        var allConnections = CreatePriorityQueue(boxes);

        var unionFind = new UnionFind(boxes.Length);
        for (var i = 0; i < this.RequiredPairs && allConnections.Count > 0; i++)
        {
            var connection = allConnections.Dequeue();
            unionFind.Union(connection.A, connection.B);
        }

        var circuitSizes = new Dictionary<int, int>();
        for (var i = 0; i < boxes.Length; i++)
        {
            var root = unionFind.Find(i);
            circuitSizes[root] = circuitSizes.GetValueOrDefault(root) + 1;
        }

        return circuitSizes.Values.OrderDescending().Take(3).Product();
    }

    public object? PartTwo(string input)
    {
        var boxes = ParseInput(input);
        var allConnections = CreatePriorityQueue(boxes);

        var unionFind = new UnionFind(boxes.Length);
        IndexPair lastConnection = default;

        var circuitCount = boxes.Length;
        while (circuitCount > 1 && allConnections.Count > 0)
        {
            var connection = allConnections.Dequeue();

            if (unionFind.Find(connection.A) != unionFind.Find(connection.B))
            {
                unionFind.Union(connection.A, connection.B);
                lastConnection = connection;
                circuitCount--;
            }
        }

        var xCoordA = (long)boxes[lastConnection.A].X;
        var xCoordB = (long)boxes[lastConnection.B].X;
        return xCoordA * xCoordB;
    }

    private static PriorityQueue<IndexPair, float> CreatePriorityQueue(ImmutableArray<Vector3> boxes)
    {
        var allConnections = new PriorityQueue<IndexPair, float>();
        for (var a = 0; a < boxes.Length; a++)
        {
            for (var b = a + 1; b < boxes.Length; b++)
            {
                var distance = Vector3.Distance(boxes[a], boxes[b]);
                allConnections.Enqueue(new(a, b), distance);
            }
        }

        return allConnections;
    }

    private static ImmutableArray<Vector3> ParseInput(string input)
    {
        var array = ImmutableArray.CreateBuilder<Vector3>();
        foreach (var line in input.EnumerateLines())
        {
            array.Add(line.ToVector3());
        }

        return array.ToImmutable();
    }

    private record struct IndexPair(int A, int B);
}
