// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using AdventOfCode.Cli.Finder;
using AdventOfCode.Cli.Runner;

internal abstract class BaseSolutionCommand : BaseCommand
{
    public BaseSolutionCommand(SolutionFinder solutionFinder, SolutionRunner solutionRunner, string name, string? description = null)
        : base(name, description)
    {
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(solutionRunner);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        this.SolutionFinder = solutionFinder;
        this.SolutionRunner = solutionRunner;
    }

    protected SolutionFinder SolutionFinder { get; private init; }

    protected SolutionRunner SolutionRunner { get; init; }
}
