// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;

public class AdventUri(IDateTimeProvider dateTimeProvider)
{
    public Uri Build(int year)
    {
        if (year < 2015 || year > dateTimeProvider.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 2015 and the current year");
        }

        return new($"https://adventofcode.com/{year}");
    }

    public Uri Build(int year, int day)
    {
        if (year < 2015 || year > dateTimeProvider.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 2015 and the current year");
        }

        if (day < 1 || day > 25)
        {
            throw new ArgumentOutOfRangeException(nameof(day), day, "Day must be between 1 and 25");
        }

        return new($"https://adventofcode.com/{year}/day/{day}");
    }
}
