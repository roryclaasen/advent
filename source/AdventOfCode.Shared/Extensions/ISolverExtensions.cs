namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ISolverExtensions
{
    public static Assembly GetAssembly(this ISolver solver)
        => solver.GetType().Assembly;

    public static ProblemAttribute GetProblemAttribute(this ISolver solver)
        => solver.GetType().GetCustomAttribute<ProblemAttribute>() ?? throw new Exception($"{nameof(ProblemAttribute)} not found on {solver.GetType().FullName}");

    public static int Year(this ISolver solver)
        => solver.GetProblemAttribute().Year;

    public static int Day(this ISolver solver)
        => solver.GetProblemAttribute().Day;

    public static string? Name(this ISolver solver)
        => solver.GetProblemAttribute().Name;

    public static Uri Uri(this ISolver solver)
        => solver.GetProblemAttribute().Uri;

    public static string GetWorkingDirectory(this ISolver solver)
        => $"Day{solver.Day()}";

    public static IOrderedEnumerable<ISolver> OrderByYearAndDay(this IEnumerable<ISolver> solvers)
        => solvers.OrderBy(s => s.Year()).ThenBy(s => s.Day());

    public static IEnumerable<IGrouping<int, ISolver>> GroupByYear(this IEnumerable<ISolver> solvers)
        => solvers.GroupBy(s => s.Year());
}
