// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using AdventOfCode.Cli.Services;

internal abstract class BaseSolutionCommand : BaseCommand
{
    public BaseSolutionCommand(ISolutionFinder solutionFinder, ISolutionRunner solutionRunner, string name, string? description = null)
        : base(name, description)
    {
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(solutionRunner);

        this.SolutionFinder = solutionFinder;
        this.SolutionRunner = solutionRunner;
    }

    protected ISolutionFinder SolutionFinder { get; private init; }

    protected ISolutionRunner SolutionRunner { get; init; }
}
