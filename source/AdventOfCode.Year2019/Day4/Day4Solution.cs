// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2019;

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared.Extensions;

[Problem(2019, 4, "Secure Container")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input)
        => ParseInput(input).Count(this.IsValidPassword);

    public object? PartTwo(string input)
        => ParseInput(input).Where(this.IsValidPassword).Count(p => p.GroupBy(c => c).Any(g => g.Count() == 2));

    private static IEnumerable<string> ParseInput(string input) => input
        .Split('-')
        .Select(int.Parse)
        .ToRange()
        .ToEnumerable()
        .Select(p => p.ToString());

    private bool IsValidPassword(string password)
    {
        var hasDouble = false;
        for (int i = 1; i < password.Length; i++)
        {
            if (password[i] < password[i - 1])
            {
                return false;
            }

            if (password[i] == password[i - 1])
            {
                hasDouble = true;
            }
        }

        return hasDouble;
    }
}
