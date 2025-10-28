// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Services;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Spectre.Console;

internal sealed class ListCommand : BaseCommand
{
    private readonly CommonOptions commonOptions;
    private readonly ISolutionFinder solutionFinder;
    private readonly AdventUri adventUri;

    public ListCommand(CommonOptions commonOptions, ISolutionFinder solutionFinder, AdventUri adventUri)
        : base("list", "List all available solutions")
    {
        ArgumentNullException.ThrowIfNull(commonOptions);
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(adventUri);

        this.commonOptions = commonOptions;
        this.solutionFinder = solutionFinder;
        this.adventUri = adventUri;

        this.Options.Add(commonOptions.Year);
    }

    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var table = new Table();
        table.AddColumn("Year");
        table.AddColumn("Day");
        table.AddColumn("Name");
        table.AddColumn("Link");

        var sortedSolvers = this.solutionFinder
            .GetSolversFor(parseResult.GetValue(this.commonOptions.Year))
            .GroupByYear()
            .OrderByDescending(y => y.Key);

        foreach (var yearSolvers in sortedSolvers)
        {
            foreach (var solver in yearSolvers.OrderBy(s => s.GetDay()))
            {
                var year = solver.GetYear();
                var day = solver.GetDay();
                var name = solver.GetName() ?? string.Empty;
                var uri = this.adventUri.Build(year, day);
                table.AddRow(year.ToString(), day.ToString(), name, $"[link={uri}]{uri}[/]");
            }

            if (yearSolvers.Key != sortedSolvers.Last().Key)
            {
                table.AddEmptyRow();
            }
        }

        AnsiConsole.Write(table);
        return ValueTask.FromResult(0);
    }
}
