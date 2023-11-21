namespace AdventOfCode.Infrastructure;

using System;

public class SolutionMissingException : Exception
{
    public SolutionMissingException(string message) : base(message)
    {
    }

    public SolutionMissingException(string message, int? year = null, int? day = null) : base(message)
    {
        Year = year;
        Day = day;
    }

    public int? Year { get; private set; }

    public int? Day { get; private set; }
}
