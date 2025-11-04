// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Services.Runner;

using System;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

internal readonly record struct ProblemPartResult(TimeSpan Elapsed, string? Expected, string? Actual, Exception? Error)
{
    [MemberNotNullWhen(true, nameof(Error))]
    public readonly bool IsError => this.Error is not null;

    [MemberNotNullWhen(true, nameof(Actual))]
    public readonly bool IsCorrect => !this.IsError && !string.IsNullOrWhiteSpace(this.Actual) && (this.Expected?.Equals(this.Actual, StringComparison.Ordinal) ?? true);

    public readonly Color ActualColor => this.IsCorrect ? Color.Green : Color.Red;

    public readonly Color ExpectedColor => string.IsNullOrWhiteSpace(this.Expected) ? Color.Yellow : this.ActualColor;
}
