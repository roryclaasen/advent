namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal sealed class ListCommand( SolutionFinder solutionFinder, AdventUri adventUri) : Command<YearSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] YearSettings settings)
    {
        var table = new Table();
        table.AddColumn("Year");
        table.AddColumn("Day");
        table.AddColumn("Name");
        table.AddColumn("Link");

        var sortedSolvers = solutionFinder
            .GetSolversFor(settings.Year)
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
        return 0;
    }
}
