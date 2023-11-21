namespace AdventOfCode.Infrastructure;

using AdventOfCode.Shared;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

internal sealed partial class Runner
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
            }
        }

        return allResults;
    }

    private Task<SolutionResult> Run(ISolver solver)
        => AnsiConsole.Status()
        .StartAsync("Initializing solution", async ctx =>
        {
            var solverName = solver.Name();
            AnsiConsole.MarkupLine(":calendar: [link={0}]{1}[/]", solver.Uri(), $"Day {solver.Day()}{(!string.IsNullOrWhiteSpace(solverName) ? $" - {solverName}" : string.Empty)}");

            ctx.Status("Loading resource files");
            var resources = await GetResourceFiles(solver).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(resources.Input))
            {
                throw new Exception("Solution input file is empty");
            }

            ctx.Status("Running part 1");
            var partOne = Solve(() => solver.PartOne(resources.Input), resources.ExpectedPartOne);
            PrintResult(1, partOne);

            ctx.Status("Running part 2");
            var partTwo = Solve(() => solver.PartTwo(resources.Input), resources.ExpectedPartTwo);
            PrintResult(2, partTwo);

            return new SolutionResult(partOne, partTwo);
        });

    private void PrintResult(int part, SolveResult result)
    {
        var resultEmoji = result.IsCorrect ? ":check_mark:" : result.IsError ? ":red_exclamation_mark:" : ":cross_mark:";

        AnsiConsole.MarkupLine($"{resultEmoji} Part {part} - {FormatTimeSpan(result.Elapsed)}");

        if (result.IsError)
        {
            AnsiConsole.WriteException(result.Error);
        }
        else
        {
            var table = new Table();
            table.AddColumn(new TableColumn("Result").Centered());

            var actualColor = result.IsCorrect ? "green" : "red";
            var actualRow = $"[{actualColor}]{result.Actual ?? "NULL"}[/]";

            if (string.IsNullOrWhiteSpace(result.Expected))
            {
                table.AddRow(actualRow);
            }
            else
            {
                table.AddColumn(new TableColumn("Expected").Centered());
                table.AddRow(actualRow, result.Expected);
            }

            AnsiConsole.Write(new Padder(table).PadLeft(3).PadTop(0).PadRight(0));
        }
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
#if DEBUG
            Debugger.Break();
#endif
            error = e;
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

