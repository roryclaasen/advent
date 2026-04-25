// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Finder;
using AdventOfCode.Cli.Runner;
using Spectre.Console;

internal sealed class TodayCommand(
    TimeProvider timeProvider,
    ISolutionFinder solutionFinder,
    ISolutionRunner solutionRunner,
    IAnsiConsole console)
    : BaseSolutionCommand(solutionFinder, solutionRunner, "today", "Run todays solution")
{
    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetLocalNow();
        if (now is { Month: 12, Day: >= 1 and <= 25 })
        {
            var solvers = this.SolutionFinder.GetSolversFor(now.Year, now.Day);
            var allResults = this.SolutionRunner.RunAll(solvers);
            var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
            return ValueTask.FromResult(exitCode);
        }

        console.MarkupLine("[red]Error:[/] Event is not active. This option works in Dec 1-25 only.");
        console.MarkupLine("[grey]Run --help to see all available commands.[/]");

        return ValueTask.FromResult(-1);
    }
}
