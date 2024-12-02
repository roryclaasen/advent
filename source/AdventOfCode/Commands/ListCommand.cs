namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal sealed class ListCommand(IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder, AdventUri adventUri) : Command<ListCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("The year of the available puzzles to list.")]
        [CommandOption("-y|--year")]
        public int? Year { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
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

    public override ValidationResult Validate([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.Year is not null)
        {
            if (settings.Year < 2015 || settings.Year > dateTimeProvider.Now.Year)
            {
                return ValidationResult.Error("Year must be between 2015 and current year.");
            }
        }

        return base.Validate(context, settings);
    }
}
