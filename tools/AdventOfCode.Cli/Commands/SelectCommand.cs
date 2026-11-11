// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Commands;

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Finder;
using AdventOfCode.Cli.Runner;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;
using Spectre.Console;

internal sealed class SelectCommand : BaseSolutionCommand
{
    private readonly IAnsiConsole console;

    public SelectCommand(SolutionFinder solutionFinder, SolutionRunner solutionRunner, IAnsiConsole console)
        : base(solutionFinder, solutionRunner, "select", "Select puzzles to run")
    {
        ArgumentNullException.ThrowIfNull(console);

        this.console = console;

        this.Options.Add(CommonOptions.Year);
        this.Options.Add(CommonOptions.Day);
    }

    protected override ValueTask<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var selectedYear = parseResult.GetValue(CommonOptions.Year);
        var selectedDay = parseResult.GetValue(CommonOptions.Day);

        if (selectedYear != 0 && selectedDay != 0)
        {
            var solvers = this.SolutionFinder.GetSolversFor(selectedYear, selectedDay);
            return RunAllSolvers(solvers);
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

        var pickedSolvers = this.console.Prompt(prompt).Where(s => s is not YearPlaceHolder);
        return RunAllSolvers(pickedSolvers);

        ValueTask<int> RunAllSolvers(IEnumerable<IProblemSolver> solvers)
        {
            var results = this.SolutionRunner.RunAll(solvers);
            return ValueTask.FromResult(SolutionResult.ToExitCode(results));
        }
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
