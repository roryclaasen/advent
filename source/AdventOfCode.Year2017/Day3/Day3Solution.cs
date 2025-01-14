namespace AdventOfCode.Year2017;

using AdventOfCode.Problem;
using System;

[Problem(2017, 3, "Spiral Memory")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var inputInt = int.Parse(input);
        var size = Math.Ceiling(Math.Sqrt(inputInt));
        var center = Math.Ceiling((size - 1) / 2);
        var steps = Math.Max(0, center - 1 + Math.Abs(center - inputInt % size));
        return steps;
    }

    public object? PartTwo(string input)
        => null;
}
