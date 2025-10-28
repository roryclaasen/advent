// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Services.Finder;

using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Cli.Services;
using AdventOfCode.Problem;

internal sealed class SolutionFinder(IEnumerable<IProblemSolver> solvers) : ISolutionFinder
{
    public IEnumerable<IProblemSolver> GetSolvers() => this.GetSolversFor();

    public IEnumerable<IProblemSolver> GetSolversFor(int year = 0, int day = 0)
    {
        if (!solvers.Any())
        {
            throw new SolutionMissingException("There are no solutions yet");
        }

#if DEBUG
        if (solvers.All(p => p.GetDay() == 0 && p.GetYear() == 0))
        {
            throw new SolutionMissingException("Looks like the solution data didn't generate. Try rebuilding the solution.");
        }
#endif

        if (day != 0 && year != 0)
        {
            var validSolvers = solvers.Where(s => s.GetYear() == year && s.GetDay() == day);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year} and day {day}", year, day);
        }
        else if (day == 0 && year != 0)
        {
            var validSolvers = solvers.Where(s => s.GetYear() == year);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year}", year);
        }
        else if (day != 0 && year == 0)
        {
            var validSolvers = solvers.Where(s => s.GetYear() == day);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the day {day}", day: day);
        }

        return solvers.OrderByYearAndDay();
    }

    public IProblemSolver GetLastSolver(int year = 0)
        => this.GetSolversFor(year: year).GroupByYear().Select(g => g.OrderByYearAndDay().Last()).Last();
}
