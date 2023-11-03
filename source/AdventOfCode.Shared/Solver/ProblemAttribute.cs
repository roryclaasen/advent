namespace AdventOfCode.Shared;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class ProblemAttribute : Attribute
{
    public ProblemAttribute(int year, int day, string? name = null)
    {
        this.Year = year;
        this.Day = day;
        this.Name = name;
    }

    public int Year { get; }

    public int Day { get; }

    public string? Name { get; }
}
