// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System.CommandLine;
using AdventOfCode.Cli;

internal static class CommonOptions
{
    internal static Option<int> Year { get; } = CreateYearOption();

    internal static Option<int> Day { get; } = CreateDayOption();

    private static Option<int> CreateYearOption()
    {
        var argument = new Option<int>("--year", "-y")
        {
            Description = "The year of the available puzzles.",
            Arity = ArgumentArity.ZeroOrOne
        };

        argument.Validators.Add(result =>
        {
            var year = result.GetRequiredValue(argument);
            if (year < 2015)
            {
                result.AddError("Year must be greater than or equal to 2015.");
            }

            var currentYear = AdventTimeProvider.Instance.GetLocalNow().Year;
            if (year > currentYear)
            {
                result.AddError($"Year must be less than or equal to {currentYear}.");
            }
        });

        return argument;
    }

    private static Option<int> CreateDayOption()
    {
        var argument = new Option<int>("--day", "-d")
        {
            Description = "The day of the available puzzle.",
            Arity = ArgumentArity.ZeroOrOne
        };

        argument.Validators.Add(result =>
        {
            var day = result.GetRequiredValue(argument);

            if (day < 1)
            {
                result.AddError("Day must be greater than or equal to 1.");
            }

            var year = result.GetValue(Year);
            if (year >= 2025)
            {
                if (day > 12)
                {
                    result.AddError("Day must be less than or equal to 12.");
                }
            }
            else if (day > 25)
            {
                result.AddError("Day must be less than or equal to 25.");
            }
        });

        return argument;
    }
}
