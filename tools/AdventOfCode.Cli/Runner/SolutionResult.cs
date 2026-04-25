// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System.Collections.Generic;

internal readonly record struct SolutionResult(PartResult Part1, PartResult Part2)
{
    public readonly bool HasError => this.Part1.IsError || this.Part2.IsError;

    public IEnumerable<PartResult> GetParts()
    {
        yield return this.Part1;
        yield return this.Part2;
    }
}
