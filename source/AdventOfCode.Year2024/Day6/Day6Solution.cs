// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

[Problem(2024, 6, "Guard Gallivant")]
public partial class Day6Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var (map, guard) = ParseInput(input);

        var (completed, visited) = Simulate(map, guard);
        if (!completed)
        {
            throw new Exception("Guard did not complete the simulation");
        }

        return visited.Count;
    }

    public object? PartTwo(string input)
    {
        var (map, guard) = ParseInput(input);
        var (_, visitedLocations) = Simulate(map, guard);

        return Task.WhenAll(visitedLocations.Select(loc => Task.Run(() =>
        {
            var mapCopy = (char[,])map.Clone();
            mapCopy[(int)loc.Y, (int)loc.X] = 'O';
            return Simulate(mapCopy, guard).Completed;
        }))).Result.Count(result => !result);
    }

    private static (bool Completed, IReadOnlySet<Vector2> VisitedLocations) Simulate(ReadOnlySpan2D<char> map, GuardInfo guard)
    {
        var visitedLocations = new HashSet<Vector2>();
        var visitedGuardInfo = new HashSet<GuardInfo>();

        while (true)
        {
            visitedLocations.Add(guard.Position);
            visitedGuardInfo.Add(new(guard.Position, guard.Direction));

            var next = guard.Position + guard.Direction.ToVector();
            if (next.X < 0 || next.X >= map.Width || next.Y < 0 || next.Y >= map.Height)
            {
                break;
            }

            if (map[(int)next.Y, (int)next.X] != '.')
            {
                guard.Direction = guard.Direction.Rotate();
                continue;
            }

            guard.Position = next;

            if (visitedGuardInfo.Contains(new(guard.Position, guard.Direction)))
            {
                return (false, visitedLocations);
            }
        }

        return (true, visitedLocations);
    }

    private static InputData ParseInput(string input)
    {
        var map = input.Lines().Select(c => c.ToCharArray()).ToArray().ToMatrix();
        for (var y = 0; y < map.GetLength(0); y++)
        {
            for (var x = 0; x < map.GetLength(1); x++)
            {
                Direction? guardDirection = map[y, x] switch
                {
                    '^' => Direction.Up,
                    '>' => Direction.Right,
                    'v' => Direction.Down,
                    '<' => Direction.Left,
                    _ => null
                };

                if (guardDirection.HasValue)
                {
                    map[y, x] = '.';
                    return new InputData(map, new GuardInfo(new Vector2(x, y), guardDirection.Value));
                }
            }
        }

        throw new InvalidOperationException("No guard found");
    }

    private record struct GuardInfo(Vector2 Position, Direction Direction);

    private record struct InputData(char[,] Map, GuardInfo Guard);
}
