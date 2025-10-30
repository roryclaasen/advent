// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Services.Runner;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Cli.Services;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;
using AdventOfCode.Shared;
using Spectre.Console;

internal sealed partial class SolutionRunner(AdventUri adventUri) : ISolutionRunner
{
    public IReadOnlyList<SolutionResult> RunAll(IEnumerable<IProblemSolver> solvers)
    {
        AnsiConsole.Write(new FigletText("Advent of Code").LeftJustified());

        var allResults = new List<SolutionResult>();
        var solversByYear = solvers.GroupBy(s => s.Year);
        foreach (var year in solversByYear.OrderBy(y => y.Key))
        {
            var yearNumber = year.First().Year;
            var rule = new Rule($"[{Color.White}][link={adventUri.Build(yearNumber)}]{yearNumber}[/][/]");
            rule.RuleStyle(Color.Olive);
            AnsiConsole.Write(rule);

            foreach (var solver in year.OrderBy(s => s.Day))
            {
                var result = this.Run(solver);
                allResults.Add(result);

                PrintResult(result);
            }
        }

        return allResults;
    }

    private static void PrintResult(SolutionResult result)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded
        };

        table.AddColumn(new TableColumn("Part").Centered());
        table.AddColumn(new TableColumn("Result").Centered());
        table.AddColumn(new TableColumn("Expected").Centered());

        void AddTableRow(int partNumber, ProblemPartResult part)
        {
            var partRow = $"[{part.ActualColor}]{(part.IsError ? "ERROR" : part.Actual ?? "NULL")}[/]";
            var partExpected = $"[{part.ExpectedColor}]{part.Expected ?? "NULL"}[/]";
            table.AddRow(partNumber.ToString(), partRow, partExpected);
        }

        AddTableRow(1, result.Part1);
        table.AddEmptyRow();
        AddTableRow(2, result.Part2);

        AnsiConsole.Write(table);
    }

    private static string FormatTimeSpan(TimeSpan timeSpan)
    {
        var color = timeSpan.TotalMilliseconds switch
        {
            < 500 => Color.Green,
            < 2 * 1000 => Color.Yellow,
            < 5 * 1000 => Color.Orange1,
            _ => Color.Red
        };

        var format = timeSpan.TotalMilliseconds switch
        {
            < 10 => $"{timeSpan.TotalMilliseconds:F2}ms",
            < 1000 => $"{timeSpan.TotalMilliseconds:F0}ms",
            < 1000 * 60 => $"{timeSpan.Seconds}s {timeSpan.Milliseconds}ms",
            _ => $"{timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms",
        };

        return $"[{color}]{format}[/]";
    }

    private static ProblemPartResult Solve(Func<string, object?> part, string? input, string? expected)
    {
        string? actual = null;
        Exception? error = null;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(input);
            actual = part.Invoke(input)?.ToString();
        }
        catch (Exception e)
        {
            error = e;
#if DEBUG
            throw;
#endif
        }

        stopwatch.Stop();
        return new ProblemPartResult(stopwatch.Elapsed, expected, actual, error);
    }

    private SolutionResult Run(IProblemSolver solver) => AnsiConsole.Status().Start("Initializing solution", ctx =>
    {
        var solverName = solver.Name;
        AnsiConsole.MarkupLine(":calendar: [link={0}]{1}[/]", adventUri.Build(solver.Year, solver.Day), solver.GetDisplayName());

        ctx.Status("Running part 1");
        var partOne = Solve(solver.PartOne, solver.Input, solver.Expected1);

        AnsiConsole.MarkupLine($"{GetResultEmoji(partOne)}  Part 1 - {FormatTimeSpan(partOne.Elapsed)}");
        if (partOne.IsError)
        {
            AnsiConsole.WriteException(partOne.Error);
        }

        ctx.Status("Running part 2");
        var partTwo = Solve(solver.PartTwo, solver.Input, solver.Expected2);

        AnsiConsole.MarkupLine($"{GetResultEmoji(partTwo)}  Part 2 - {FormatTimeSpan(partTwo.Elapsed)}");
        if (partTwo.IsError)
        {
            AnsiConsole.WriteException(partTwo.Error);
        }

        return new SolutionResult(partOne, partTwo);

        static string GetResultEmoji(ProblemPartResult result)
            => result.IsCorrect ? ":check_mark:" : result.IsError ? ":red_exclamation_mark:" : ":cross_mark:";
    });
}
