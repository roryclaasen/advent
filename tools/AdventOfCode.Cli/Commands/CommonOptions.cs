// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System.CommandLine;
using System.CommandLine.Completions;
using System.CommandLine.Parsing;
using System.Linq;
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

        argument.CompletionSources.Add(_ => Enumerable.Range(2015, AdventTimeProvider.Instance.GetLocalNow().Year - 2015 + 1).Select(year => new CompletionItem(year.ToString())));

        argument.Validators.Add(result =>
        {
            if (!TryGetValue(result, out var year))
            {
                result.AddError("Year must be an integer.");
                return;
            }

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

        argument.CompletionSources.Add(context =>
        {
            if (!TryGetValue(context.ParseResult.GetResult(Year), out var year))
            {
                return [];
            }

            return Enumerable.Range(1, year >= 2025 ? 12 : 25).Select(day => new CompletionItem(day.ToString(), sortText: day.ToString("D2")));
        });

        argument.Validators.Add(result =>
        {
            if (!TryGetValue(result, out var day))
            {
                result.AddError("Day must be an integer.");
                return;
            }

            if (day < 1)
            {
                result.AddError("Day must be greater than or equal to 1.");
            }

            if (!TryGetValue(result.GetResult(Year), out var year))
            {
                return;
            }

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

    private static bool TryGetValue(OptionResult? result, out int value)
    {
        value = 0;
        return result is null || result.Tokens.Count == 0 || int.TryParse(result.Tokens[0].Value, out value);
    }
}
