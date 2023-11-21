namespace AdventOfCode;

using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AdventOfCode.Infrastructure;
using AdventOfCode.Shared;

internal sealed class ListCommand(IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder) : Command<ListCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("The year of the available puzzles to list.")]
        [CommandOption("-y|--year")]
        public int? Year { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var solvers = solutionFinder.GetSolversFor(settings.Year).GroupByYear();

        var table = new Table();
        table.AddColumn("Year");
        table.AddColumn("Day");
        table.AddColumn("Name");
        table.AddColumn("Link");

        foreach (var yearSolvers in solvers.OrderBy(y => y.Key))
        {
            foreach (var solver in yearSolvers.OrderBy(s => s.Day()))
            {
                var year = solver.Year().ToString();
                var day = solver.Day().ToString();
                var name = solver.Name() ?? string.Empty;
                var uri = solver.Uri();
                table.AddRow(year, day, name, $"[link={uri}]{uri}[/]");
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