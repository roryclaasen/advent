namespace AdventOfCode;

using AdventOfCode.Shared;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static partial class Runner
{
    public static async Task RunAll(IEnumerable<ISolver> solvers)
    {
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
                await Run(solver);
            }
        }
    }

    private static Task Run(ISolver solver)
        => AnsiConsole.Status()
        .StartAsync("Initializing solution", async ctx =>
        {
            var solverName = solver.Name();
            AnsiConsole.MarkupLine(":calendar: [link={0}]{1}[/]", solver.Uri().AbsoluteUri, $"Day {solver.Day()}{(!string.IsNullOrWhiteSpace(solverName) ? $" - {solverName}" : string.Empty)}");

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
        });

    private static void PrintResult(int part, SolveResult result)
    {
        var resultEmoji = result.IsCorrect ? ":check_mark:" : result.IsError ? ":red_exclamation_mark:" : ":cross_mark:";

        AnsiConsole.MarkupLine($"{resultEmoji} Part {part} - {result.Elapsed.TotalMilliseconds}ms");

        if (result.IsError)
        {
            AnsiConsole.WriteException(result.Error);
        }
        else
        {
            var table = new Table();
            table.AddColumn(new TableColumn("Result").Centered());

            var actualResult = result.Actual ?? "NULL";
            if (result.IsCorrect)
            {
                table.AddRow($"[green]{actualResult}[/]");
            }
            else if (!string.IsNullOrWhiteSpace(result.Expected))
            {
                table.AddColumn(new TableColumn("Expected").Centered());
                table.AddRow($"[red]{result.Actual}[/]", result.Expected);
            }
            else if (string.IsNullOrWhiteSpace(result.Actual))
            {
                table.AddRow($"[red]NULL[/]");
            }
            else
            {
                throw new UnreachableException();
            }

            AnsiConsole.Write(new Padder(table).PadLeft(3).PadTop(0).PadRight(0));
        }
    }

    private static SolveResult Solve(Func<object?> action, string? expected)
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

    private static async Task<ResourceFiles> GetResourceFiles(ISolver solver)
    {
        var assembly = solver.GetAssembly();
        var workingDirectory = solver.GetWorkingDirectory();

        var input = await ReadResourceFile(assembly, Path.Combine(workingDirectory, "input.txt")).ConfigureAwait(false);
        var expectedPartOne = await ReadExpectedResourceFile(assembly, Path.Combine(workingDirectory, "expected1.txt")).ConfigureAwait(false);
        var expectedPartTwo = await ReadExpectedResourceFile(assembly, Path.Combine(workingDirectory, "expected2.txt")).ConfigureAwait(false);

        return new ResourceFiles(input, expectedPartOne, expectedPartTwo);
    }

    private static async Task<string?> ReadExpectedResourceFile(Assembly assembly, string inputFile)
    {
        try
        {
            var file = await ReadResourceFile(assembly, inputFile).ConfigureAwait(false);
            return string.IsNullOrWhiteSpace(file) ? null : file;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private static async Task<string> ReadResourceFile(Assembly assembly, string filePath)
    {
        var input = await assembly.ReadResourceFile(filePath).ConfigureAwait(false);
        var normalized = NewLineRegex().Replace(input, Environment.NewLine);
        if (normalized.EndsWith(Environment.NewLine, StringComparison.Ordinal))
        {
            normalized = normalized[..^Environment.NewLine.Length];
        }

        return normalized;
    }

    private record ResourceFiles(string Input, string? ExpectedPartOne, string? ExpectedPartTwo);

    [GeneratedRegex("\\r\\n|\\n\\r|\\n|\\r")]
    private static partial Regex NewLineRegex();
}

public record SolveResult(TimeSpan Elapsed, string? Expected, string? Actual, Exception? Error)
{
    [MemberNotNullWhen(true, nameof(Actual))]
    public bool IsCorrect => Error is null && !string.IsNullOrWhiteSpace(Actual) && (Expected?.Equals(Actual, StringComparison.Ordinal) ?? true);

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError => Error is not null;
}

