namespace AdventOfCode.Infrastructure;

using AdventOfCode.Problem;
using System.Collections.Generic;
using System.Linq;

internal sealed class SolutionFinder(IEnumerable<IProblemSolver> solvers)
{
    public IEnumerable<IProblemSolver> GetSolvers() => GetSolversFor(null, null);

    public IEnumerable<IProblemSolver> GetSolversFor(int? year = null, int? day = null)
    {
        if (!solvers.Any())
        {
            throw new SolutionMissingException("There are no solutions yet");
        }

        if (day is not null && year is not null)
        {
            var validSolvers = solvers.Where(s => s.GetYear() == year && s.GetDay() == day);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year} and day {day}", year, day);
        }
        else if (day is null && year is not null)
        {
            var validSolvers = solvers.Where(s => s.GetYear() == year);
            if (validSolvers.Any())
            {
                return validSolvers.OrderByYearAndDay();
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year}", year);
        }
        else if (day is not null && year is null)
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

    public IProblemSolver GetLastSolver(int? year = null)
        => GetSolversFor(year).GroupByYear().Select(g => g.OrderByYearAndDay().Last()).Last();
}
