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
    public AllCommand(CommonOptions commonOptions, ISolutionFinder solutionFinder, ISolutionRunner solutionRunner)
        : base(commonOptions, solutionFinder, solutionRunner, "all", "Run all the solutions")
    {
        this.Options.Add(commonOptions.Year);
        this.Options.Add(commonOptions.Day);
    }

    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var year = parseResult.GetValue(this.CommonOptions.Year);
        var day = parseResult.GetValue(this.CommonOptions.Day);

        var solvers = this.SolutionFinder.GetSolversFor(year, day);
        var allResults = this.SolutionRunner.RunAll(solvers);
        var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
        return ValueTask.FromResult(exitCode);
    }
}
