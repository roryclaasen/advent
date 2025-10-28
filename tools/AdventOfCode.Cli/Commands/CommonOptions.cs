// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using System.CommandLine;
using AdventOfCode.Shared;

internal sealed class CommonOptions(IDateTimeProvider dateTimeProvider)
{
    private readonly Lazy<Option<int>> lazyYear = new(() =>
    {
        var argument = new Option<int>("--year")
        {
            Description = "The year of the available puzzles."
        };

        argument.Validators.Add(result =>
        {
            var year = result.GetRequiredValue(argument);
            if (year < 2015 || year > dateTimeProvider.Now.Year)
            {
                result.AddError("Year must be between 2015 and current year.");
            }
        });

        return argument;
    });

    private readonly Lazy<Option<int>> lazyDay = new(() =>
    {
        var argument = new Option<int>("--day")
        {
            Description = "The day of the available puzzle (1-25)."
        };

        argument.Validators.Add(result =>
        {
            var day = result.GetRequiredValue(argument);
            if (day < 1 || day > 25)
            {
                result.AddError("Day must be between 1 and 25.");
            }
        });

        return argument;
    });

    internal Option<int> Year => this.lazyYear.Value;

    internal Option<int> Day => this.lazyDay.Value;
}
