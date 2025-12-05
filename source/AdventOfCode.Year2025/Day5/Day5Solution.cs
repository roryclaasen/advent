// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Problem;
using AdventOfCode.Shared.Numerics;

[Problem(2025, 5, "Cafeteria")]
public partial class Day5Solution : IProblemSolver
{
    [GeneratedRegex(@"^(\d+)-(\d+)$")]
    private static partial Regex RangeRegex { get; }

    public object? PartOne(string input)
    {
        var database = ParseInput(input);

        var freshIngredients = 0;

        foreach (var ingredient in database.AvailableIngredients)
        {
            foreach (var range in database.Ranges)
            {
                if (range.Contains(ingredient))
                {
                    freshIngredients++;
                    break;
                }
            }
        }

        return freshIngredients;
    }

    public object? PartTwo(string input)
    {
        var database = ParseInput(input);
        var currentRanges = new HashSet<SimpleRange<long>>();

        foreach (var range in database.Ranges)
        {
            var combined = range;

            foreach (var existing in currentRanges)
            {
                if (combined.Intersects(existing))
                {
                    combined = combined.Combine(existing);
                    currentRanges.Remove(existing);
                }
            }

            currentRanges.Add(combined);
        }

        return currentRanges.Sum(r => r.Length);
    }

    private static Database ParseInput(string input)
    {
        var ranges = ImmutableArray.CreateBuilder<SimpleRange<long>>();
        var ingredients = ImmutableArray.CreateBuilder<long>();
        foreach (var line in input.EnumerateLines())
        {
            if (line.Length == 0)
            {
                continue;
            }

            if (RangeRegex.Match(line.ToString()) is { Success: true } match)
            {
                var start = long.Parse(match.Groups[1].Value);
                var end = long.Parse(match.Groups[2].Value);
                ranges.Add(new SimpleRange<long>(start, end));
            }
            else if (long.TryParse(line, out var ingredient))
            {
                ingredients.Add(ingredient);
            }
            else
            {
                throw new InvalidOperationException($"Could not parse line: {line}");
            }
        }

        return new Database(ranges.ToImmutable(), ingredients.ToImmutable());
    }

    private record struct Database(ImmutableArray<SimpleRange<long>> Ranges, ImmutableArray<long> AvailableIngredients);
}
