// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;

internal sealed class SolutionRunner(SolutionResultRenderer renderer)
{
    public IReadOnlyList<SolutionResult> RunAll(IEnumerable<IProblemSolver> solvers)
    {
        renderer.Banner();

        var solversList = solvers.ToList();
        if (solversList.Count == 0)
        {
            renderer.NoSolutions();
            return [];
        }

        var results = new List<SolutionResult>(solversList.Count);

        foreach (var year in solversList.OrderByYearAndDay().GroupByYear())
        {
            renderer.Year(year.Key);

            foreach (var solver in year)
            {
                var result = renderer.Run(
                    solver,
                    () => SolvePart(solver.PartOne, solver.Input, solver.Expected1),
                    () => SolvePart(solver.PartTwo, solver.Input, solver.Expected2));

                results.Add(result);
            }
        }

        renderer.Summary(results);
        return results;
    }

    private static PartResult SolvePart(Func<string, object?> part, string? input, string? expected)
    {
        string? actual = null;
        Exception? error = null;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(input);
            actual = part(input)?.ToString();
        }
        catch (Exception e)
        {
            error = e;
#if DEBUG
            throw;
#endif
        }
        finally
        {
            stopwatch.Stop();
        }

        return new PartResult(stopwatch.Elapsed, expected, actual, error);
    }
}
