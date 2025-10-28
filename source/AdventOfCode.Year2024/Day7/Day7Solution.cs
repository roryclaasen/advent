// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Extensions;

[Problem(2024, 7, "Bridge Repair")]
public partial class Day7Solution : IProblemSolver
{
    public object? PartOne(string input) => ParseInput(input).Sum(equation =>
    {
        var results = GetAllPossibleResults(false, equation.Values.First(), equation.Values.Skip(1));
        if (results.Any(r => r == equation.Result))
        {
            return (long)equation.Result;
        }

        return 0;
    });

    public object? PartTwo(string input) => Task.WhenAll(ParseInput(input).Select(e => Task.Run(() =>
    {
        var results = GetAllPossibleResults(true, e.Values.First(), e.Values.Skip(1));
        if (results.Any(r => r == e.Result))
        {
            return (long)e.Result;
        }

        return 0;
    }))).Result.Sum();

    private static IEnumerable<ulong> GetAllPossibleResults(bool allowConcatenation, ulong left, params IEnumerable<ulong> rights)
    {
        if (!rights.Any())
        {
            yield return left;
            yield break;
        }

        var next = rights.First();
        var nextRights = rights.Skip(1);

        foreach (var result in GetAllPossibleResults(allowConcatenation, left + next, nextRights))
        {
            yield return result;
        }

        foreach (var result in GetAllPossibleResults(allowConcatenation, left * next, nextRights))
        {
            yield return result;
        }

        if (allowConcatenation)
        {
            foreach (var result in GetAllPossibleResults(true, MathUtils.FastConcat(left, next), nextRights))
            {
                yield return result;
            }
        }
    }

    private static IEnumerable<Equation> ParseInput(string input)
    {
        foreach (var equation in input.Lines())
        {
            var (result, values) = equation.Split(':');
            var resultInt = ulong.Parse(result.Trim());
            var valuesInt = values.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray();
            yield return new Equation(resultInt, valuesInt);
        }
    }

    private record struct Equation(ulong Result, params ulong[] Values);
}
