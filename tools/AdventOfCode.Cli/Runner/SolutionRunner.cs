// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;

internal sealed class SolutionRunner(SolutionResultRenderer renderer)
{
    public IReadOnlyList<SolutionResult> RunAll(IEnumerable<IProblemSolver> solvers, CancellationToken cancellationToken)
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
                cancellationToken.ThrowIfCancellationRequested();

                var result = renderer.Run(
                    solver,
                    () => SolvePart(solver.PartOne, solver.Input, solver.Expected1, cancellationToken),
                    () => SolvePart(solver.PartTwo, solver.Input, solver.Expected2, cancellationToken));

                results.Add(result);
            }
        }

        renderer.Summary(results);
        return results;
    }

    private static PartResult SolvePart(Func<string, object?> part, string? input, string? expected, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string? actual = null;
        Exception? error = null;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(input);
            actual = part.Invoke(input)?.ToString();
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
