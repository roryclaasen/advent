namespace AdventOfCode;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

public static class ISolverExtensions
{
    public static int Year(this ISolver solver)
        => solver is IProblemDetails details ? details.Year : 0;

    public static int Day(this ISolver solver)
        => solver is IProblemDetails details ? details.Day : 0;

    public static string? Name(this ISolver solver)
        => solver is IProblemDetails details ? details.Name : null;

    public static IOrderedEnumerable<ISolver> OrderByYearAndDay(this IEnumerable<ISolver> solvers)
        => solvers.OrderBy(s => s.Year()).ThenBy(s => s.Day());

    public static IEnumerable<IGrouping<int, ISolver>> GroupByYear(this IEnumerable<ISolver> solvers)
        => solvers.GroupBy(s => s.Year());
}
