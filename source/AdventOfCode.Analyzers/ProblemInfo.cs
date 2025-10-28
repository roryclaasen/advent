// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

internal readonly record struct ProblemInfo
{
    public readonly string Namespace;
    public readonly string ClassName;
    public readonly int Year;
    public readonly int Day;
    public readonly string? Name;

    public ProblemInfo(string Namespace, string className, int year, int day, string? name)
    {
        this.Namespace = Namespace;
        this.ClassName = className;
        this.Year = year;
        this.Day = day;
        this.Name = name;
    }
}
