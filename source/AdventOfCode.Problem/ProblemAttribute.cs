namespace AdventOfCode.Problem;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class ProblemAttribute(int year, int day, string? name = null) : Attribute
{
    public int Year { get; } = year;

    public int Day { get; } = day;

    public string? Name { get; } = name;
}
