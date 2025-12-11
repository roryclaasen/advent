// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2025, 11, "Reactor")]
public partial class Day11Solution : IProblemSolver
{
    private static readonly string[] RequiredNodes = ["dac", "fft"];

    public object? PartOne(string input) => CountPaths("you", "out", ParseInput(input), []);

    public object? PartTwo(string input) => CountPathsWithRequired("svr", "out", ParseInput(input), RequiredNodes);

    private static long CountPaths(string current, string target, Dictionary<string, HashSet<string>> graph, Dictionary<string, long> memo)
    {
        if (current == target)
        {
            return 1;
        }

        if (memo.TryGetValue(current, out var cached))
        {
            return cached;
        }

        if (!graph.TryGetValue(current, out var outputs))
        {
            return 0;
        }

        long count = 0;
        foreach (var next in outputs)
        {
            count += CountPaths(next, target, graph, memo);
        }

        memo[current] = count;
        return count;
    }

    private static long CountPathsWithRequired(string start, string target, Dictionary<string, HashSet<string>> graph, string[] requiredNodes)
    {
        var requiredIndex = new Dictionary<string, int>();
        for (int i = 0; i < requiredNodes.Length; i++)
        {
            requiredIndex[requiredNodes[i]] = i;
        }

        int allVisitedMask = (1 << requiredNodes.Length) - 1;
        var memo = new Dictionary<Memoization, long>();
        return CountWithRequired(start, target, 0);

        long CountWithRequired(string current, string target, int visitedMask)
        {
            if (requiredIndex.TryGetValue(current, out var bitIndex))
            {
                visitedMask |= 1 << bitIndex;
            }

            if (current == target)
            {
                return visitedMask == allVisitedMask ? 1 : 0;
            }

            var key = new Memoization(current, visitedMask);
            if (memo.TryGetValue(key, out var cached))
            {
                return cached;
            }

            if (!graph.TryGetValue(current, out var outputs))
            {
                return 0;
            }

            long count = 0;
            foreach (var next in outputs)
            {
                count += CountWithRequired(next, target, visitedMask);
            }

            memo[key] = count;
            return count;
        }
    }

    private static Dictionary<string, HashSet<string>> ParseInput(string input)
    {
        var dictionary = new Dictionary<string, HashSet<string>>();

        foreach (var line in input.Lines())
        {
            var deviceEndIndex = line.IndexOf(':');
            var device = line[..deviceEndIndex];

            dictionary[device] = [.. line[(deviceEndIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries)];
        }

        return dictionary;
    }

    private record struct Memoization(string Device, int VisitedMask);
}
