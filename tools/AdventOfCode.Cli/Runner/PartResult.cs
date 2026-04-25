// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System;
using System.Diagnostics.CodeAnalysis;

internal readonly record struct PartResult(TimeSpan Elapsed, string? Expected, string? Actual, Exception? Error)
{
    [MemberNotNullWhen(true, nameof(Error))]
    public readonly bool IsError => this.Error is not null;

    [MemberNotNullWhen(true, nameof(Actual))]
    public readonly bool IsCorrect => !this.IsError && !string.IsNullOrWhiteSpace(this.Actual) && (this.Expected?.Equals(this.Actual, StringComparison.Ordinal) ?? true);

    public Verdict Verdict {
        get {
            if (this.IsError)
            {
                return Verdict.Error;
            }

            if (string.IsNullOrWhiteSpace(this.Expected))
            {
                return Verdict.Unknown;
            }

            return this.IsCorrect ? Verdict.Pass : Verdict.Fail;
        }
    }
}
