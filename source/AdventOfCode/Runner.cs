namespace AdventOfCode;

using AdventOfCode.Shared;
using Kurukuru;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class Runner
{
    public static async Task RunAll(IEnumerable<ISolver> solvers)
    {
        var solversByYear = solvers.GroupBy(s => s.Year());
        foreach (var year in solversByYear.OrderBy(y => y.Key))
        {
            PrintHeading(year.First());
            foreach (var solver in year.OrderBy(s => s.Day()))
            {
                await Run(solver);
            }
        }
    }

    public static async Task Run(ISolver solver)
    {
        await Console.Out.WriteLineAsync($"{solver.Day()}: {solver.Name() ?? string.Empty}");

        var resources = await GetResourceFiles(solver).ConfigureAwait(false);

        var partOne = await SolveSpinner("  Part 1", () => solver.PartOne(resources.Input), resources.ExpectedPartOne).ConfigureAwait(false);
        PrintResult(partOne);

        var partTwo = await SolveSpinner("  Part 2", () => solver.PartTwo(resources.Input), resources.ExpectedPartTwo).ConfigureAwait(false);
        PrintResult(partTwo);
    }

    private static void PrintHeading(ISolver solver)
    {
        var line = new string('=', 80);
        Console.WriteLine(line);
        Console.WriteLine($"Advent of Code {solver.Year()}");
        Console.WriteLine(line);
    }

    private static void PrintResult(SolveResult result)
    {
        var actual = result.Actual?.ToString() ?? "NULL";
        var isMultiLine = actual.Contains(Environment.NewLine);
        if (result.IsCorrect)
        {
            if (isMultiLine)
            {
                Console.WriteLine("    Result:");
                Console.WriteLine(actual);
            }
            else
            {
                Console.WriteLine($"    Result: {actual}");
            }

        }
        else if (result.Error is not null)
        {
            Console.WriteLine(result.Error.StackTrace);
        }
        else if (result.Expected is not null)
        {
            if (isMultiLine)
            {
                Console.WriteLine("    Expected:");
                Console.WriteLine(result.Expected);
                Console.WriteLine("    Actual:");
                Console.WriteLine(result.Actual);
            }
            else
            {
                Console.WriteLine($"    Expected: {result.Expected} Actual: {result.Actual}");
            }
        }
        else if (result.Actual is null)
        {
            Console.WriteLine($"    There was no answer provided for this solution.");
        }
        else
        {
            throw new UnreachableException();
        }

        Console.WriteLine();
    }

    private static Task<SolveResult> SolveSpinner(string prefix, Func<object?> action, string? expected)
        => Spinner.StartAsync(prefix, (spinner) =>
        {
            var result = Solve(action, expected);
            if (result.IsCorrect)
            {
                spinner.Succeed($"{prefix} ({result.Elapsed.TotalMilliseconds}ms)");
            }
            else if (result.Error is not null)
            {
                spinner.Fail($"{prefix} ({result.Elapsed.TotalMilliseconds}ms) - {result.Error.Message}");
            }
            else
            {
                spinner.Fail($"{prefix} ({result.Elapsed.TotalMilliseconds}ms) - Incorrect result");
            }

            return Task.FromResult(result);
        });

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

        var isCorrect = error is null && actual is not null && (expected?.Equals(actual, StringComparison.Ordinal) ?? true);
        return new SolveResult(stopwatch.Elapsed, expected, actual, isCorrect, error);
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
        var normalized = Regex.Replace(input, @"\r\n|\n\r|\n|\r", Environment.NewLine);
        if (normalized.EndsWith(Environment.NewLine, StringComparison.Ordinal))
        {
            normalized = normalized[..^Environment.NewLine.Length];
        }

        return normalized;
    }

    private record ResourceFiles(string Input, string? ExpectedPartOne, string? ExpectedPartTwo);
}

public record SolveResult(TimeSpan Elapsed, string? Expected, string? Actual, bool IsCorrect, Exception? Error);
