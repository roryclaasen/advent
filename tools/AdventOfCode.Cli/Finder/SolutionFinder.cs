// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Finder;

using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;

internal sealed class SolutionFinder(IEnumerable<IProblemSolver> solvers)
{
    private readonly IProblemSolver[] solvers = [.. solvers];

    public IEnumerable<IProblemSolver> GetSolvers() => this.GetSolversFor();

    public IEnumerable<IProblemSolver> GetSolversFor(int year = 0, int day = 0)
    {
        if (this.solvers.Length == 0)
        {
            throw new SolutionMissingException("There are no solutions yet");
        }

#if DEBUG
        if (this.solvers.All(p => p.Day == 0 && p.Year == 0))
        {
            throw new SolutionMissingException("Looks like the solution data didn't generate. Try rebuilding the solution.");
        }
#endif

        var matched = this.solvers
            .Where(s => (year == 0 || s.Year == year) && (day == 0 || s.Day == day))
            .OrderByYearAndDay()
            .ToList();

        if (matched.Count > 0)
        {
            return matched;
        }

        var message = (year, day) switch
        {
            (0, 0) => "There are no solutions yet",
            (_, 0) => $"There are no solutions yet for the year {year}",
            (0, _) => $"There are no solutions yet for the day {day}",
            _ => $"There are no solutions yet for the year {year} and day {day}",
        };

        throw new SolutionMissingException(message, year == 0 ? null : year, day == 0 ? null : day);
    }

    public IProblemSolver GetLastSolver(int year = 0) => this.GetSolversFor(year: year).Last();
}
