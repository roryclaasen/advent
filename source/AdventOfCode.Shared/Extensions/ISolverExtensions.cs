namespace AdventOfCode.Shared;

using System.Reflection;

public static class ISolverExtensions
{
    public static Assembly GetAssembly(this ISolver solver)
        => solver.GetType().Assembly;
}
