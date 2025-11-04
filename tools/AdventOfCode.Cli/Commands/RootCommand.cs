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
using AdventOfCode.Problem.Extensions;
using Spectre.Console;
using BaseRootCommand = System.CommandLine.RootCommand;

internal sealed class RootCommand : BaseRootCommand
{
    private readonly ISolutionFinder solutionFinder;
    private readonly ISolutionRunner solutionRunner;

    public RootCommand(
        ListCommand listCommand,
        AllCommand allCommand,
        TodayCommand todayCommand,
        LastCommand lastCommand,
        ISolutionFinder solutionFinder,
        ISolutionRunner solutionRunner)
        : base("Rory Claasens solutions to Advent of Code")
    {
        ArgumentNullException.ThrowIfNull(listCommand);
        ArgumentNullException.ThrowIfNull(allCommand);
        ArgumentNullException.ThrowIfNull(todayCommand);
        ArgumentNullException.ThrowIfNull(lastCommand);
        ArgumentNullException.ThrowIfNull(solutionFinder);
        ArgumentNullException.ThrowIfNull(solutionRunner);

        this.Subcommands.Add(listCommand);
        this.Subcommands.Add(allCommand);
        this.Subcommands.Add(todayCommand);
        this.Subcommands.Add(lastCommand);

        this.solutionFinder = solutionFinder;
        this.solutionRunner = solutionRunner;

        this.Options.Add(CommonOptions.Year);
        this.Options.Add(CommonOptions.Day);

        this.SetAction(async (parseResult, cancellationToken) => await this.ExecuteAsync(parseResult, cancellationToken));
    }

    private ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var selectedYear = parseResult.GetValue(CommonOptions.Year);
        var selectedDay = parseResult.GetValue(CommonOptions.Day);

        if (selectedYear != 0 && selectedDay != 0)
        {
            var solver = this.solutionFinder.GetSolversFor(selectedYear, selectedDay);
            var exitCode = this.solutionRunner.RunAll(solver).Any(r => r.HasError) ? -1 : 0;
            return ValueTask.FromResult(exitCode);
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
            foreach (var year in this.solutionFinder.GetSolvers().GroupByYear().OrderByDescending(g => g.Key))
            {
                prompt.AddChoiceGroup(new YearPlaceHolder(year.Key), year.OrderByYearAndDay());
            }
        }
        else
        {
            prompt.AddChoiceGroup(new YearPlaceHolder(selectedYear), this.solutionFinder.GetSolversFor(selectedYear).OrderByYearAndDay());
        }

        var pickedSolvers = AnsiConsole.Prompt(prompt);

        var allResults = this.solutionRunner.RunAll(pickedSolvers.Where(s => s is not YearPlaceHolder));
        var isError = allResults.Any(r => r.HasError);
        return ValueTask.FromResult(isError ? -1 : 0);
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
