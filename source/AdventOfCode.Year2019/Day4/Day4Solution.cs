namespace AdventOfCode.Year2019;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

[Problem(2019, 4, "Secure Container")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input)
        => ParseInput(input).Count(IsValidPassword);

    public object? PartTwo(string input)
        => ParseInput(input).Where(IsValidPassword).Count(p => p.GroupBy(c=>c).Any(g => g.Count() == 2));

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

    private static IEnumerable<string> ParseInput(string input) => input
        .Split('-')
        .Select(int.Parse)
        .ToRange()
        .ToEnumerable()
        .Select(p => p.ToString());
}
