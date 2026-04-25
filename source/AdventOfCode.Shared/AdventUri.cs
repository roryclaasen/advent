// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;

public class AdventUri(TimeProvider timeProvider)
{
    public Uri Build(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 2015, nameof(year));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(year, timeProvider.GetLocalNow().Year, nameof(year));

        return new($"https://adventofcode.com/{year}");
    }

    public Uri Build(int year, int day)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 2015, nameof(year));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(year, timeProvider.GetLocalNow().Year, nameof(year));

        ArgumentOutOfRangeException.ThrowIfLessThan(day, 1, nameof(day));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(day, 25, nameof(day));

        return new($"https://adventofcode.com/{year}/day/{day}");
    }
}
