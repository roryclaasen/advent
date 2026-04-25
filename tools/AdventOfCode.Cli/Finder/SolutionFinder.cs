// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Finder;

using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;

internal sealed class SolutionFinder(IEnumerable<IProblemSolver> solvers) : ISolutionFinder
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

        if (day != 0 && year != 0)
        {
            var validSolvers = this.solvers.Where(s => s.Year == year && s.Day == day);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year} and day {day}", year, day);
        }
        else if (day == 0 && year != 0)
        {
            var validSolvers = this.solvers.Where(s => s.Year == year);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year}", year);
        }
        else if (day != 0 && year == 0)
        {
            var validSolvers = this.solvers.Where(s => s.Day == day);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the day {day}", day: day);
        }

        return this.solvers.OrderByYearAndDay();
    }

    public IProblemSolver GetLastSolver(int year = 0)
        => this.GetSolversFor(year: year).OrderByYearAndDay().Last();
}
