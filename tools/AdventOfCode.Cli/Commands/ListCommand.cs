// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Finder;
using AdventOfCode.Problem.Extensions;
using AdventOfCode.Shared;
using Spectre.Console;

internal sealed class ListCommand : BaseCommand
{
    private readonly ISolutionFinder solutionFinder;
    private readonly AdventUri adventUri;
    private readonly IAnsiConsole console;

    public ListCommand(ISolutionFinder solutionFinder, AdventUri adventUri, IAnsiConsole console)
        : base("list", "List all available solutions")
    {
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(adventUri);
        ArgumentNullException.ThrowIfNull(console);

        this.solutionFinder = solutionFinder;
        this.adventUri = adventUri;
        this.console = console;

        this.Options.Add(CommonOptions.Year);
    }

    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var solversByYear = this.solutionFinder
            .GetSolversFor(parseResult.GetValue(CommonOptions.Year))
            .GroupByYear()
            .OrderByDescending(g => g.Key)
            .ToList();

        if (solversByYear.Count == 0)
        {
            this.console.MarkupLine("[yellow]No matching solutions found.[/]");
            return ValueTask.FromResult(0);
        }

        var tree = new Tree("[bold green]:christmas_tree: Advent of Code Solutions[/]")
        {
            Style = new Style(Color.Grey),
            Guide = TreeGuide.Line,
        };

        foreach (var yearGroup in solversByYear)
        {
            var year = yearGroup.Key;
            var yearUri = this.adventUri.Build(year);
            var dayCount = yearGroup.Count();
            var yearNode = tree.AddNode(
                $"[bold yellow][link={yearUri}]{year}[/][/] [grey]({dayCount} day{(dayCount == 1 ? string.Empty : "s")})[/]");

            foreach (var solver in yearGroup.OrderBy(s => s.Day))
            {
                var dayUri = this.adventUri.Build(solver.Year, solver.Day);
                var name = string.IsNullOrWhiteSpace(solver.Name)
                    ? string.Empty
                    : $"  [grey]{Markup.Escape(solver.Name)}[/]";
                yearNode.AddNode($"[link={dayUri}][bold]Day {solver.Day,2}[/][/]{name}");
            }
        }

        this.console.Write(tree);
        return ValueTask.FromResult(0);
    }
}
