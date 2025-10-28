// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using AdventOfCode.Cli.Infrastructure.Runner;
using AdventOfCode.Cli.Infrastructure.SolutionFinder;
using System;

internal abstract class BaseSolutionCommand : BaseCommand
{
    public BaseSolutionCommand(Options options, SolutionFinder solutionFinder, Runner solutionRunner, string name, string? description = null)
        : base(name, description)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(solutionRunner);

        this.CommandOptions = options;
        this.SolutionFinder = solutionFinder;
        this.SolutionRunner = solutionRunner;
    }

    protected Options CommandOptions { get; private init; }

    protected SolutionFinder SolutionFinder { get; private init; }

    protected Runner SolutionRunner { get; init; }
}
