namespace AdventOfCode.Infrastructure;

using AdventOfCode.Shared;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

internal sealed partial class Runner(AdventUri uriHelper)
{
    public async Task<IReadOnlyList<SolutionResult>> RunAll(IEnumerable<ISolver> solvers)
    {
        var allResults = new List<SolutionResult>();
        var solversByYear = solvers.GroupBy(s => s.Year());
        foreach (var year in solversByYear.OrderBy(y => y.Key))
        {
            var heading = new FigletText(year.First().Year().ToString())
                .LeftJustified()
                .Color(Color.Yellow);

            AnsiConsole.Write(new Panel(heading)
            {
                Header = new PanelHeader("Advent of Code")
            });

            foreach (var solver in year.OrderBy(s => s.Day()))
            {
                var result = await Run(solver).ConfigureAwait(false);
                allResults.Add(result);

                PrintResult(result);
            }
        }

        return allResults;
    }

    private Task<SolutionResult> Run(ISolver solver)
        => AnsiConsole.Status()
        .StartAsync("Initializing solution", async ctx =>
        {
            var solverName = solver.Name();
            AnsiConsole.MarkupLine(":calendar: [link={0}]{1}[/]", uriHelper.Build(solver.Year(), solver.Day()), $"Day {solver.Day()}{(!string.IsNullOrWhiteSpace(solverName) ? $" - {solverName}" : string.Empty)}");

            ctx.Status("Loading resource files");
            var resources = await GetResourceFiles(solver).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(resources.Input))
            {
                throw new Exception("Solution input file is empty");
            }

            ctx.Status("Running part 1");
            var partOne = Solve(() => solver.PartOne(resources.Input), resources.ExpectedPartOne);

            AnsiConsole.MarkupLine($"{this.GetResultEmoji(partOne)} Part 1 - {FormatTimeSpan(partOne.Elapsed)}");
            if (partOne.IsError)
            {
                AnsiConsole.WriteException(partOne.Error);
            }

            ctx.Status("Running part 2");
            var partTwo = Solve(() => solver.PartTwo(resources.Input), resources.ExpectedPartTwo);

            AnsiConsole.MarkupLine($"{this.GetResultEmoji(partTwo)} Part 2 - {FormatTimeSpan(partTwo.Elapsed)}");
            if (partTwo.IsError)
            {
                AnsiConsole.WriteException(partTwo.Error);
            }

            return new SolutionResult(partOne, partTwo);
        });

    private string GetResultEmoji(SolveResult result)
        => result.IsCorrect ? ":check_mark:" : result.IsError ? ":red_exclamation_mark:" : ":cross_mark:";

    private void PrintResult(SolutionResult result)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded
        };

        table.AddColumn(new TableColumn("Part").Centered());
        table.AddColumn(new TableColumn("Result").Centered());
        table.AddColumn(new TableColumn("Expected").Centered());

        var partOneColor = result.Part1.IsCorrect ? "green" : "red";
        var partOneRow = $"[{partOneColor}]{(result.Part1.IsError ? "ERROR" : result.Part1.Actual ?? "NULL")}[/]";
        var partOneExpected = $"[{partOneColor}]{result.Part1.Expected ?? "NULL"}[/]";
        table.AddRow("1", partOneRow, partOneExpected);

        table.AddEmptyRow();

        var partTwoColor = result.Part2.IsCorrect ? "green" : "red";
        var partTwoRow = $"[{partTwoColor}]{(result.Part2.IsError ? "ERROR" : result.Part2.Actual ?? "NULL")}[/]";
        var partTwoExpected = $"[{partTwoColor}]{result.Part2.Expected ?? "NULL"}[/]";
        table.AddRow("2", partTwoRow, partTwoExpected);

        AnsiConsole.Write(table);
    }

    private static string FormatTimeSpan(TimeSpan timeSpan)
    {
        var color = timeSpan.TotalMilliseconds switch
        {
            < 1000 => "green",
            < 2000 => "yellow",
            _ => "red"
        };

        var format = timeSpan.TotalMilliseconds switch
        {
            < 1000 => $"{timeSpan.TotalMilliseconds}ms",
            < 1000 * 60 => $"{timeSpan.Seconds}s {timeSpan.Milliseconds}ms",
            _ => $"{timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms",
        };

        return $"[{color}]{format}[/]";
    }

    private SolveResult Solve(Func<object?> action, string? expected)
    {
        string? actual = null;
        Exception? error = null;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            actual = action()?.ToString();
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
}

