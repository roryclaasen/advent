// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2015, 24, "It Hangs in the Balance")]
public partial class Day24Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var packages = ParseInput(input).ToArray();
        return FindBestQE(packages, 3);
    }

    public object? PartTwo(string input)
    {
        var packages = ParseInput(input).ToArray();
        return FindBestQE(packages, 4);
    }

    private static long FindBestQE(int[] packages, int groups)
    {
        var targetWeight = packages.Sum() / groups;

        for (var i = 0; i < packages.Length; i++)
        {
            var parts = PickPackages(packages, i, 0, targetWeight);
            if (parts.Any())
            {
                return parts.Select(l => l.Product()).Min();
            }
        }

        throw new Exception("No solution found");
    }

    private static IEnumerable<ImmutableList<long>> PickPackages(int[] packages, int count, int i, int targetWeight)
    {
        if (targetWeight == 0)
        {
            yield return ImmutableList.Create<long>();
            yield break;
        }

        if (count < 0 || targetWeight < 0 || i >= packages.Length)
        {
            yield break;
        }

        if (packages[i] <= targetWeight)
        {
            foreach (var x in PickPackages(packages, count - 1, i + 1, targetWeight - packages[i]))
            {
                yield return x.Add(packages[i]);
            }
        }

        foreach (var x in PickPackages(packages, count, i + 1, targetWeight))
        {
            yield return x;
        }
    }

    private static IEnumerable<int> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            yield return int.Parse(line);
        }
    }
}
