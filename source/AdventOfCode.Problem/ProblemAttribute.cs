// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Problem;

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ProblemAttribute(int year, int day, string? name = null) : Attribute
{
    public int Year { get; } = year;

    public int Day { get; } = day;

    public string? Name { get; } = name;
}
