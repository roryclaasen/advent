// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

internal readonly record struct ProblemInfo
{
    public ProblemInfo(string classNamespace, string className, int year, int day, string? name)
    {
        this.Namespace = classNamespace;
        this.ClassName = className;
        this.Year = year;
        this.Day = day;
        this.Name = name;
    }

    public readonly string Namespace { get; }

    public readonly string ClassName { get; }

    public readonly int Year { get; }

    public readonly int Day { get; }

    public readonly string? Name { get; }
}
