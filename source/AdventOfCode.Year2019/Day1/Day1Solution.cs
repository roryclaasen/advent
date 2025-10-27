// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2019;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Linq;

[Problem(2019, 1, "The Tyranny of the Rocket Equation")]
public partial class Day1Solution : IProblemSolver
{
    public object? PartOne(string input)
        => input.Lines().Select(int.Parse).Sum(Fuel);

    public object? PartTwo(string input)
    {
        return input.Lines().Select(int.Parse).Sum(x =>
        {
            var s = 0;
            for (var y = Fuel(x); y > 0; y = Fuel(y))
            {
                s += y;
            }
            return s;
        });
    }

    private static int Fuel(int x) => x / 3 - 2;
}
