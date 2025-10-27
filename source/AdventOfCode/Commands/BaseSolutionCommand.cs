namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
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
