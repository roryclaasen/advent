namespace AdventOfCode.Shared;

using System.Collections.Generic;
using System.Linq;

public static class ISolverWithDetailsExtensions
{
    public static IOrderedEnumerable<ISolverWithDetails> OrderByYearAndDay(this IEnumerable<ISolverWithDetails> solvers)
        => solvers.OrderBy(s => s.Year).ThenBy(s => s.Day);

    public static IEnumerable<IGrouping<int, ISolverWithDetails>> GroupByYear(this IEnumerable<ISolverWithDetails> solvers)
        => solvers.GroupBy(s => s.Year);

    public static string GetWorkingDirectory(this ISolverWithDetails solver)
        => $"Day{solver.Day}";
}

