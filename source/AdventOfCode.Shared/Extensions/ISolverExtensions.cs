namespace AdventOfCode.Shared;

using System.Reflection;

public static class ISolverExtensions
{
    public static Assembly GetAssembly(this ISolver solver)
        => solver.GetType().Assembly;

    public static string? GetExpectedResultPart1(this ISolver solver)
        => solver.GetConstantStringField("Expected1");

    public static string? GetExpectedResultPart2(this ISolver solver)
        => solver.GetConstantStringField("Expected2");

    private static string? GetConstantStringField(this ISolver solver, string fieldName)
    {
        var fieldInfo = solver
            .GetType()
            .GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
        var value = fieldInfo?.GetValue(solver)?.ToString();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
