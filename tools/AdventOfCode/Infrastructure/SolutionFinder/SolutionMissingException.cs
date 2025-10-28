// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Infrastructure;

using System;

public class SolutionMissingException : Exception
{
    public SolutionMissingException(string message)
        : base(message)
    {
    }

    public SolutionMissingException(string message, int? year = null, int? day = null)
        : base(message)
    {
        this.Year = year;
        this.Day = day;
    }

    public int? Year { get; private set; }

    public int? Day { get; private set; }
}
