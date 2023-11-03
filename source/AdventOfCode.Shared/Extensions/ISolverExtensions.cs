namespace AdventOfCode.Shared;

using System;
using System.Reflection;

public static class ISolverExtensions
{
    public static Assembly GetAssembly(this ISolver solver) => solver.GetType().Assembly;

    public static ProblemAttribute GetProblemAttribute(this ISolver solver)
        => solver.GetType().GetCustomAttribute<ProblemAttribute>() ?? throw new Exception($"ProblemAttribute not found on {solver.GetType().FullName}");

    public static int Year(this ISolver solver)
        => solver.GetProblemAttribute().Year;

    public static int Day(this ISolver solver)
        => solver.GetProblemAttribute().Day;

    public static string? Name(this ISolver solver)
        => solver.GetProblemAttribute().Name;

    public static string GetWorkingDirectory(this ISolver solver)
        => $"Day{solver.Day()}";

    public static int Year(Type type)
    {
        var fullName = type.FullName ?? throw new Exception($"Type.FullName is null for {type}");
        return int.Parse(fullName.Split('.')[1][4..]);
    }

    public static int Day(Type type)
    {
        var fullName = type.FullName ?? throw new Exception($"Type.FullName is null for {type}");
        return int.Parse(fullName.Split('.')[2][3..^8]);
    }
}
