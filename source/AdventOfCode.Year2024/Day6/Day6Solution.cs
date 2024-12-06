namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[Problem(2024, 6, "Guard Gallivant")]
public partial class Day6Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var (map, guard) = ParseInput(input);

        var (completed, visited, _) = Simulate(map, guard);
        if (!completed)
        {
            throw new Exception("Guard did not complete the simulation");
        }

        return visited.Count;
    }

    public object? PartTwo(string input)
    {
        var (map, guard) = ParseInput(input);
        var (_, initalVisitedLocations, initialGuardInfo) = Simulate(map, guard);

        var totalLoops = 0;

        //var width = map.GetLength(1);
        //var height = map.GetLength(0);
        //foreach (var initGuard in initialGuardInfo)
        //{
        //    var next = initGuard.Direction.ToVector() + initGuard.Position;
        //    if (next.X < 0 || next.X >= width || next.Y < 0 || next.Y >= height)
        //    {
        //        continue;
        //    }

        //    var mapCopy = (char[,])map.Clone();
        //    mapCopy[(int)next.Y, (int)next.X] = 'O';
        //    var (completed, _, _) = Simulate(mapCopy, initGuard);
        //    if (!completed)
        //    {
        //        totalLoops++;
        //    }
        //}

        foreach (var visitedLoc in initalVisitedLocations)
        {
            var mapCopy = (char[,])map.Clone();
            mapCopy[(int)visitedLoc.Y, (int)visitedLoc.X] = 'O';
            var (completed, _, _) = Simulate(mapCopy, guard);
            if (!completed)
            {
                totalLoops++;
            }
        }

        return totalLoops;
    }

    private (bool Completed, IReadOnlySet<Vector2> VisitedLocations, IReadOnlySet<GuardInfo>) Simulate(ReadOnlySpan2D<char> map, GuardInfo guard)
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
                return (false, visitedLocations, visitedGuardInfo);
            }
        }

        return (true, visitedLocations, visitedGuardInfo);
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
