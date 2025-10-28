// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Tests;

using System.Diagnostics.CodeAnalysis;
using AdventOfCode.Problem;

public abstract class SolverBaseTests<TSolver> where TSolver : IProblemSolver, new()
{
    protected TSolver Solver { get; private set; }

    [TestInitialize]
    [MemberNotNull(nameof(this.Solver))]
    public virtual void SetUp()
    {
        this.Solver = new TSolver();
    }
}
