// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class AllCommand : BaseSolutionCommand
{
    public AllCommand(Options options, SolutionFinder solutionFinder, Runner solutionRunner)
        : base(options, solutionFinder, solutionRunner, "all", "Run all the solutions")
    {
        this.Options.Add(options.Year);
        this.Options.Add(options.Day);
    }

    protected override Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var year = parseResult.GetValue(this.CommandOptions.Year);
        var day = parseResult.GetValue(this.CommandOptions.Day);

        var solvers = this.SolutionFinder.GetSolversFor(year, day);
        var allResults = this.SolutionRunner.RunAll(solvers);
        var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
        return Task.FromResult(exitCode);
    }
}
