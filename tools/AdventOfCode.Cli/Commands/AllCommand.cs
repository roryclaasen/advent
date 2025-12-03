// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Services;

internal sealed class AllCommand : BaseSolutionCommand
{
    public AllCommand(ISolutionFinder solutionFinder, ISolutionRunner solutionRunner)
        : base(solutionFinder, solutionRunner, "all", "Run all the solutions")
    {
        this.Options.Add(CommonOptions.Year);
        this.Options.Add(CommonOptions.Day);
    }

    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var year = parseResult.GetValue(CommonOptions.Year);
        var day = parseResult.GetValue(CommonOptions.Day);

        var solvers = this.SolutionFinder.GetSolversFor(year, day);
        var allResults = this.SolutionRunner.RunAll(solvers);
        var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
        return ValueTask.FromResult(exitCode);
    }
}
