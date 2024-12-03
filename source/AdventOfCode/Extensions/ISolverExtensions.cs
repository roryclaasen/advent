namespace AdventOfCode;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

public static class ISolverExtensions
{
    public static int GetYear(this ISolver solver)
        => solver is IProblemDetails problem ? problem.Year : 0;

    public static int GetDay(this ISolver solver)
        => solver is IProblemDetails problem ? problem.Day : 0;

    public static string? GetName(this ISolver solver)
        => solver is IProblemDetails problem ? problem.Name : null;

    public static IOrderedEnumerable<ISolver> OrderByYearAndDay(this IEnumerable<ISolver> solvers)
        => solvers.OrderBy(s => s.GetYear()).ThenBy(s => s.GetDay());

    public static IEnumerable<IGrouping<int, ISolver>> GroupByYear(this IEnumerable<ISolver> solvers)
        => solvers.GroupBy(s => s.GetYear());

    public static string? GetInput(this ISolver solver)
        => solver is IProblemInput problem ? problem.Input : null;

    public static string? GetExpectedResultPart1(this ISolver solver)
        => solver is IProblemExpectedResultPart1 problem ? problem.Expected1 : null;

    public static string? GetExpectedResultPart2(this ISolver solver)
        => solver is IProblemExpectedResultPart2 problem ? problem.Expected2 : null;
}
