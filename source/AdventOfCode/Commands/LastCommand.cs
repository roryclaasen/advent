// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class LastCommand : BaseSolutionCommand
{
    public LastCommand(Options options, SolutionFinder solutionFinder, Runner solutionRunner)
        : base(options, solutionFinder, solutionRunner, "last", "Run the last solution for a given year")
    {
        this.Options.Add(options.Year);
    }

    protected override Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var solver = this.SolutionFinder.GetLastSolver(parseResult.GetValue(this.CommandOptions.Year));
        var allResults = this.SolutionRunner.RunAll([solver]);
        var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
        return Task.FromResult(exitCode);
    }
}
