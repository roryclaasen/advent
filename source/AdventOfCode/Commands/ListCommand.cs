namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Spectre.Console;
using System;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class ListCommand : BaseCommand
{
    private readonly Options commandOptions;
    private readonly SolutionFinder solutionFinder;
    private readonly AdventUri adventUri;

    public ListCommand(Options options, SolutionFinder solutionFinder, AdventUri adventUri)
        : base("list", "List all available solutions")
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(adventUri);

        this.commandOptions = options;
        this.solutionFinder = solutionFinder;
        this.adventUri = adventUri;

        this.Options.Add(options.Year);
    }

    protected override Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var table = new Table();
        table.AddColumn("Year");
        table.AddColumn("Day");
        table.AddColumn("Name");
        table.AddColumn("Link");

        var sortedSolvers = this.solutionFinder
            .GetSolversFor(parseResult.GetValue(this.commandOptions.Year))
            .GroupByYear()
            .OrderByDescending(y => y.Key);

        foreach (var yearSolvers in sortedSolvers)
        {
            foreach (var solver in yearSolvers.OrderBy(s => s.GetDay()))
            {
                var year = solver.GetYear();
                var day = solver.GetDay();
                var name = solver.GetName() ?? string.Empty;
                var uri = adventUri.Build(year, day);
                table.AddRow(year.ToString(), day.ToString(), name, $"[link={uri}]{uri}[/]");
            }

            if (yearSolvers.Key != sortedSolvers.Last().Key)
            {
                table.AddEmptyRow();
            }
        }

        AnsiConsole.Write(table);
        return Task.FromResult(0);
    }
}
