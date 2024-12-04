namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Linq;

internal class DefaultCommand(SolutionFinder solutionFinder, Runner solutionRunner) : Command<YearAndDaySettings>
{
    public override int Execute(CommandContext context, YearAndDaySettings settings)
    {
        if (settings.Year is not null && settings.Day is not null)
        {
            var solver = solutionFinder.GetSolversFor(settings.Year, settings.Day);
            return solutionRunner.RunAll(solver).Any(r => r.HasError) ? -1 : 0;
        }

        var prompt = new MultiSelectionPrompt<IProblemSolver>()
            .Title("Which puzzles do you want to run?")
            .UseConverter(this.SolverNameConverter)
            .Required()
            .PageSize(20)
            .MoreChoicesText("[grey](Move up and down to reveal more puzzles)[/]")
            .InstructionsText("[grey](Press [blue]<space>[/] to toggle a puzzle to run, [green]<enter>[/] to accept)[/]");

        if (settings.Year is null)
        {
            foreach (var year in solutionFinder.GetSolvers().GroupByYear().OrderByDescending(g => g.Key))
            {
                prompt.AddChoiceGroup(new YearPlaceHolder(year.Key), year.OrderByYearAndDay());
            }
        }
        else
        {
            prompt.AddChoiceGroup(new YearPlaceHolder(settings.Year.Value), solutionFinder.GetSolversFor(settings.Year).OrderByYearAndDay());
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

    private sealed class YearPlaceHolder(int year) : IProblemSolver
    {
        public int Year => year;

        public object? PartOne(string input) => throw new NotImplementedException();

        public object? PartTwo(string input) => throw new NotImplementedException();
    }
}
