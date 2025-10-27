// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2023;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2023, 8, "Haunted Wasteland")]
public partial class Day8Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var (directions, mappings) = ParseInput(input);
        var steps = 0;
        var current = "AAA";
        do
        {
            var direction = directions[steps % directions.Length];
            current = GetNextKey(current, mappings, direction);
            steps++;
        } while (current != "ZZZ");
        return steps;
    }

    public object? PartTwo(string input)
    {
        var (directions, mappings) = ParseInput(input);

        var allPaths = new List<long>();
        foreach (var node in mappings.Keys.Where(s => s.EndsWith('A')))
        {
            var steps = 0;
            var current = node;
            do
            {
                var direction = directions[steps % directions.Length];
                current = GetNextKey(current, mappings, direction);
                steps++;
            } while (!current.EndsWith('Z'));
            allPaths.Add(steps);
        }
        return MathUtils.Lcm(allPaths);
    }

    private static string GetNextKey(string current, Dictionary<string, (string Left, string Right)> mappings, Direction direction) => direction switch
    {
        Direction.Left => mappings[current].Left,
        Direction.Right => mappings[current].Right,
        _ => throw new Exception("Invalid direction")
    };

    private static (Direction[] Directions, Dictionary<string, (string Left, string Right)> Mapping) ParseInput(string input)
    {
        var parts = input.Lines(2);
        var directions = parts[0].ToCharArray().Select(c => (Direction)c);
        if (directions.Any(d => d == Direction.Up || d == Direction.Down))
        {
            throw new Exception("Invalid input (directions)");
        }

        var mappings = new Dictionary<string, (string, string)>();
        foreach (var line in parts[1].Lines())
        {
            var match = NodeMapRegex().Match(line);
            if (!match.Success)
            {
                throw new Exception($"Invalid input '{line}'");
            }

            var source = match.Groups[1].Value;
            var left = match.Groups[2].Value;
            var right = match.Groups[3].Value;
            mappings.Add(source, (left, right));
        }

        return (directions.ToArray(), mappings);
    }

    [GeneratedRegex(@"(\w{3}) = \((\w{3}), (\w{3})\)")]
    private static partial Regex NodeMapRegex();
}
