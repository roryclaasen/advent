namespace AdventOfCode.Infrastructure;

using AdventOfCode.Shared;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal sealed partial class Runner(AdventUri adventUri)
{
    public IReadOnlyList<SolutionResult> RunAll(IEnumerable<IProblemSolver> solvers)
    {
        AnsiConsole.Write(new FigletText("Advent of Code").LeftJustified());

        var allResults = new List<SolutionResult>();
        var solversByYear = solvers.GroupBy(s => s.GetYear());
        foreach (var year in solversByYear.OrderBy(y => y.Key))
        {
            var yearNumber = year.First().GetYear();
            var rule = new Rule($"[{Color.White}][link={adventUri.Build(yearNumber)}]{yearNumber}[/][/]");
            rule.RuleStyle(Color.Olive);
            AnsiConsole.Write(rule);

            foreach (var solver in year.OrderBy(s => s.GetDay()))
            {
                var result = Run(solver);
                allResults.Add(result);

                PrintResult(result);
            }
        }

        return allResults;
    }

    private SolutionResult Run(IProblemSolver solver)
        => AnsiConsole.Status()
        .Start("Initializing solution", ctx =>
        {
            var solverName = solver.GetName();
            AnsiConsole.MarkupLine(":calendar: [link={0}]{1}[/]", adventUri.Build(solver.GetYear(), solver.GetDay()), $"Day {solver.GetDay()}{(!string.IsNullOrWhiteSpace(solverName) ? $" - {solverName}" : string.Empty)}");

            ctx.Status("Running part 1");
            var partOne = Solve(solver.PartOne, solver.GetInput(), solver.GetExpectedResultPart1());

            AnsiConsole.MarkupLine($"{GetResultEmoji(partOne)} Part 1 - {FormatTimeSpan(partOne.Elapsed)}");
            if (partOne.IsError)
            {
                AnsiConsole.WriteException(partOne.Error);
            }

            ctx.Status("Running part 2");
            var partTwo = Solve(solver.PartTwo, solver.GetInput(), solver.GetExpectedResultPart2());

            AnsiConsole.MarkupLine($"{GetResultEmoji(partTwo)} Part 2 - {FormatTimeSpan(partTwo.Elapsed)}");
            if (partTwo.IsError)
            {
                AnsiConsole.WriteException(partTwo.Error);
            }

            return new SolutionResult(partOne, partTwo);
        });

    private static string GetResultEmoji(SolveResult result)
        => result.IsCorrect ? ":check_mark:" : result.IsError ? ":red_exclamation_mark:" : ":cross_mark:";

    private static void PrintResult(SolutionResult result)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded
        };

        table.AddColumn(new TableColumn("Part").Centered());
        table.AddColumn(new TableColumn("Result").Centered());
        table.AddColumn(new TableColumn("Expected").Centered());

        void AddTableRow(int partNumber, SolveResult part)
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
            < 1000 => $"{(int)timeSpan.TotalMilliseconds}ms",
            < 1000 * 60 => $"{timeSpan.Seconds}s {timeSpan.Milliseconds}ms",
            _ => $"{timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms",
        };

        return $"[{color}]{format}[/]";
    }

    private static SolveResult Solve(Func<string, object?> part, string? input, string? expected)
    {
        string? actual = null;
        Exception? error = null;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            ArgumentNullException.ThrowIfNull(input);
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
        return new SolveResult(stopwatch.Elapsed, expected, actual, error);
    }
}

public record SolutionResult(SolveResult Part1, SolveResult Part2)
{
    public bool HasError => this.Part1.IsError || this.Part2.IsError;
}

public record SolveResult(TimeSpan Elapsed, string? Expected, string? Actual, Exception? Error)
{
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError => this.Error is not null;

    [MemberNotNullWhen(true, nameof(Actual))]
    public bool IsCorrect => !this.IsError && !string.IsNullOrWhiteSpace(this.Actual) && (this.Expected?.Equals(this.Actual, StringComparison.Ordinal) ?? true);

    public Color ActualColor => this.IsCorrect ? Color.Green : Color.Red;

    public Color ExpectedColor => string.IsNullOrWhiteSpace(this.Expected) ? Color.Yellow : this.ActualColor;
}

