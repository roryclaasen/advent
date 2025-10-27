namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using Spectre.Console;
using System;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal class PickCommand : BaseSolutionCommand
{
    public PickCommand(Options options, SolutionFinder solutionFinder, Runner solutionRunner)
        : base(options, solutionFinder, solutionRunner, "pick", "Runs selected puzzles.")
    {
        this.Options.Add(options.Year);
        this.Options.Add(options.Day);
    }

    protected override Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var selectedYear = parseResult.GetValue(this.CommandOptions.Year);
        var selectedDay = parseResult.GetValue(this.CommandOptions.Day);

        if (selectedYear != 0 && selectedDay != 0)
        {
            var solver = this.SolutionFinder.GetSolversFor(selectedYear, selectedDay);
            var exitCode = this.SolutionRunner.RunAll(solver).Any(r => r.HasError) ? -1 : 0;
            return Task.FromResult(exitCode);
        }

        var prompt = new MultiSelectionPrompt<IProblemSolver>()
            .Title("Which puzzles do you want to run?")
            .UseConverter(this.SolverNameConverter)
            .Required()
            .PageSize(20)
            .MoreChoicesText("[grey](Move up and down to reveal more puzzles)[/]")
            .InstructionsText("[grey](Press [blue]<space>[/] to toggle a puzzle to run, [green]<enter>[/] to accept)[/]");

        if (selectedYear == 0)
        {
            foreach (var year in this.SolutionFinder.GetSolvers().GroupByYear().OrderByDescending(g => g.Key))
            {
                prompt.AddChoiceGroup(new YearPlaceHolder(year.Key), year.OrderByYearAndDay());
            }
        }
        else
        {
            prompt.AddChoiceGroup(new YearPlaceHolder(selectedYear), this.SolutionFinder.GetSolversFor(selectedYear).OrderByYearAndDay());
        }

        var pickedSolvers = AnsiConsole.Prompt(prompt);

        var allResults = this.SolutionRunner.RunAll(pickedSolvers.Where(s => s is not YearPlaceHolder));
        var isError = allResults.Any(r => r.HasError);
        return Task.FromResult(isError ? -1 : 0);
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
