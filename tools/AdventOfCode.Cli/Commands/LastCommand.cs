// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Services;

internal sealed class LastCommand : BaseSolutionCommand
{
    public LastCommand(ISolutionFinder solutionFinder, ISolutionRunner solutionRunner)
        : base(solutionFinder, solutionRunner, "last", "Run the last solution for a given year")
    {
        this.Options.Add(CommonOptions.Year);
    }

    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var solver = this.SolutionFinder.GetLastSolver(parseResult.GetValue(CommonOptions.Year));
        var allResults = this.SolutionRunner.RunAll([solver]);
        var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
        return ValueTask.FromResult(exitCode);
    }
}
