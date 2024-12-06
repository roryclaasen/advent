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
        //foreach (var initGuard in initialGuardInfo)
        //{
        //    var y = (int)initGuard.Position.Y;
        //    var x = (int)initGuard.Position.X;

        //    if (map[y,x] != '.')
        //    {
        //        continue;
        //    }


        //    var mapCopy = (char[,])map.Clone();
        //    mapCopy[y, x] = 'O';
        //    var (completed, _, _) = Simulate(mapCopy, initialGuardInfo.ElementAt(index - 1));
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

            var next = guard.Position + GetOffset(guard.Direction);
            if (next.X < 0 || next.X >= map.Width || next.Y < 0 || next.Y >= map.Height)
            {
                break;
            }

            if (map[(int)next.Y, (int)next.X] != '.')
            {
                guard.Direction = guard.Direction switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    _ => throw new NotImplementedException()
                };

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

    private static Vector2 GetOffset(Direction direction) => direction switch
    {
        Direction.Up => new Vector2(0, -1),
        Direction.Right => new Vector2(1, 0),
        Direction.Down => new Vector2(0, 1),
        Direction.Left => new Vector2(-1, 0),
        _ => throw new NotImplementedException()
    };

    private record struct GuardInfo(Vector2 Position, Direction Direction);

    private record struct InputData(char[,] Map, GuardInfo Guard);
}
