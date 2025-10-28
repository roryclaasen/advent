// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Services;

using System.Collections.Generic;
using AdventOfCode.Problem;

internal interface ISolutionFinder
{
    IEnumerable<IProblemSolver> GetSolvers();

    IEnumerable<IProblemSolver> GetSolversFor(int year = 0, int day = 0);

    IProblemSolver GetLastSolver(int year = 0);
}
