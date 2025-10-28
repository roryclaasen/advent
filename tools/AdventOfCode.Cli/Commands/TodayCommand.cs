// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Services;
using AdventOfCode.Shared;
using Spectre.Console;

internal sealed class TodayCommand(CommonOptions commonOptions, IDateTimeProvider dateTimeProvider, ISolutionFinder solutionFinder, ISolutionRunner solutionRunner)
    : BaseSolutionCommand(commonOptions, solutionFinder, solutionRunner, "today", "Run todays solution")
{
    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var now = dateTimeProvider.AoCNow;
        if (now is { Month: 12, Day: >= 1 and <= 25 })
        {
            var solvers = this.SolutionFinder.GetSolversFor(now.Year, now.Day);
            var allResults = this.SolutionRunner.RunAll(solvers);
            var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
            return ValueTask.FromResult(exitCode);
        }

        AnsiConsole.MarkupLine($"[{Color.Red}]Error:[/] Event is not active. This option works in Dec 1-25 only.");
        AnsiConsole.MarkupLine("Run --help to see all available commands.");

        return ValueTask.FromResult(-1);
    }
}
