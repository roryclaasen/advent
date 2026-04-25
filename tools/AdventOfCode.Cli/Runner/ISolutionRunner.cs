// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System.Collections.Generic;
using AdventOfCode.Problem;

internal interface ISolutionRunner
{
    IReadOnlyList<SolutionResult> Run(IProblemSolver solver) => this.RunAll([solver]);

    IReadOnlyList<SolutionResult> RunAll(IEnumerable<IProblemSolver> solvers);
}
