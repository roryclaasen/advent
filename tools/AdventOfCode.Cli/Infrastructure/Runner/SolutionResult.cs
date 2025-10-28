// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Infrastructure.Runner;

internal record struct SolutionResult(ProblemPartResult Part1, ProblemPartResult Part2)
{
    public readonly bool HasError => this.Part1.IsError || this.Part2.IsError;
}
