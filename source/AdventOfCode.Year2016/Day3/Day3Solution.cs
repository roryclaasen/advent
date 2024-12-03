namespace AdventOfCode.Year2016;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2016, 3, "Squares With Three Sides")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input)
        => ParseInput(input)
            .Count(IsValidTriangle);

    public object? PartTwo(string input)
    {
        var inputArray = ParseInput(input).ToArray();

        var validTriangles = 0;
        for (var i = 0; i < inputArray.Length; i += 3)
        {
            var triangle1 = new int[3]
            {
                inputArray[i][0],
                inputArray[i + 1][0],
                inputArray[i + 2][0]
            };
            var triangle2 = new int[3]
            {
                inputArray[i][1],
                inputArray[i + 1][1],
                inputArray[i + 2][1]
            };
            var triangle3 = new int[3]
            {
                inputArray[i][2],
                inputArray[i + 1][2],
                inputArray[i + 2][2]
            };

            validTriangles += IsValidTriangle(triangle1) ? 1 : 0;
            validTriangles += IsValidTriangle(triangle2) ? 1 : 0;
            validTriangles += IsValidTriangle(triangle3) ? 1 : 0;
        }

        return validTriangles;
    }

    private bool IsValidTriangle(int[] sides)
        => sides[0] + sides[1] > sides[2] &&
               sides[1] + sides[2] > sides[0] &&
               sides[2] + sides[0] > sides[1];

    private static IEnumerable<int[]> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var sides = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            yield return new int[3]
            {
                int.Parse(sides[0]),
                int.Parse(sides[1]),
                int.Parse(sides[2])
            };
        }
    }
}
