// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015;

using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared.Extensions;

[Problem(2015, 20, "Infinite Elves and Infinite Houses")]
public partial class Day20Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var target = int.Parse(input) / 10;

        for (int i = 0; i < target; i++)
        {
            var factors = i.GetFactors();
            if (factors.Sum() >= target)
            {
                return i;
            }
        }

        return null;
    }

    public object? PartTwo(string input)
    {
        var target = int.Parse(input) / 11;

        for (int i = 0; i < target; i++)
        {
            var factors = i.GetFactors().Where(d => i / d <= 50);
            if (factors.Sum() >= target)
            {
                return i;
            }
        }

        return null;
    }
}
