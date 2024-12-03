namespace AdventOfCode.Shared;

using System.Collections.Generic;
using System.Linq;

public static class ISolverWithDetailsExtensions
{
    public static IOrderedEnumerable<IProblemDetails> OrderByYearAndDay(this IEnumerable<IProblemDetails> solvers)
        => solvers.OrderBy(s => s.Year).ThenBy(s => s.Day);

    public static IEnumerable<IGrouping<int, IProblemDetails>> GroupByYear(this IEnumerable<IProblemDetails> solvers)
        => solvers.GroupBy(s => s.Year);

    public static string GetWorkingDirectory(this IProblemDetails solver)
        => $"Day{solver.Day}";
}

