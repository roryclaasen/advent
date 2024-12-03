namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal class PickCommand(IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder, Runner solutionRunner) : Command<PickCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("The year of the available puzzles to list.")]
        [CommandOption("-y|--year")]
        public int? Year { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var prompt = new MultiSelectionPrompt<IProblemSolver>()
            .Title("Which puzzles do you want to run?")
            .UseConverter(this.SolverNameConverter)
            .Required()
            .PageSize(20)
            .MoreChoicesText("[grey](Move up and down to reveal more puzzles)[/]")
            .InstructionsText("[grey](Press [blue]<space>[/] to toggle a puzzle to run, [green]<enter>[/] to accept)[/]");

        if (settings.Year is null)
        {
            foreach (var year in solutionFinder.GetSolvers().GroupByYear())
            {
                prompt.AddChoiceGroup(new YearPlaceHolder(year.Key), year.OrderByYearAndDay());
            }
        }
        else
        {
            prompt.AddChoices(solutionFinder.GetSolversFor(settings.Year).OrderByYearAndDay());
        }

        var pickedSolvers = AnsiConsole.Prompt(prompt);

        var allResults = solutionRunner.RunAll(pickedSolvers.Where(s => s is not YearPlaceHolder));
        var isError = allResults.Any(r => r.HasError);
        return isError ? -1 : 0;
    }

    private string SolverNameConverter(IProblemSolver solver)
    {
        if (solver is YearPlaceHolder yearPlaceHolder)
        {
            return $"Year {yearPlaceHolder.Year}";
        }

        return solver.GetDisplayName();
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

    private sealed class YearPlaceHolder(int year) : IProblemSolver
    {
        public int Year => year;

        public object? PartOne(string input) => throw new NotImplementedException();

        public object? PartTwo(string input) => throw new NotImplementedException();
    }
}
