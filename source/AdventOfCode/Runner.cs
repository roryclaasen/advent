namespace AdventOfCode;

using AdventOfCode.Shared;
using Kurukuru;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Runner
{
    private static Task<SolveResult> SpinnerTask(int part, Task<SolveResult> partTask)
    {
        var prefix = $"Part {part}";
        return Spinner.StartAsync(prefix, async (spinner) =>
        {
            var result = await partTask;
            var messagePrefix = $"{prefix} ({result.Elapsed.TotalMilliseconds}ms)";
            if (result.IsCorrect)
            {
                spinner.Succeed(messagePrefix);
            }
            else if (result.Error is not null)
            {
                spinner.Fail($"{messagePrefix} - {result.Error.Message}");
            }
            else
            {
                spinner.Fail($"{messagePrefix} - Incorrect, expected {result.Expected} but got {result.Actual}");
            }

            return result;
        });
    }

    public static async Task RunAll(IEnumerable<ISolver> solvers)
    {
        var runner = new Runner();
        foreach (var solver in solvers)
        {
            var name = solver.Name();
            await Console.Out.WriteLineAsync($"Year {solver.Year()} Day {solver.Day()}{(name is not null ? $" - {name}" : string.Empty)}");
            var (partOne, partTwo) = await runner.Execute(solver);
            var partOneResult = await SpinnerTask(1, partOne);
            if (partOneResult.IsCorrect)
            {
                await Console.Out.WriteLineAsync(partOneResult.Actual);
            }
            var partTwoResult = await SpinnerTask(1, partTwo);
            if (partTwoResult.IsCorrect)
            {
                await Console.Out.WriteLineAsync(partTwoResult.Actual);
            }
        }
    }

    private async Task<(Task<SolveResult> PartOne, Task<SolveResult> PartTwo)> Execute(ISolver solver)
    {
        var resourceFiles = await this.GetResourceFiles(solver).ConfigureAwait(false);
        var partOne = Task.Run(() => this.Solve(() => solver.PartOne(resourceFiles.Input), resourceFiles.ExpectedPartOne));
        var partTwo = Task.Run(() => this.Solve(() => solver.PartTwo(resourceFiles.Input), resourceFiles.ExpectedPartTwo));
        return (partOne, partTwo);
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
        }
        stopwatch.Stop();

        var isCorrect = error is null && actual is not null && (expected?.Equals(actual, StringComparison.Ordinal) ?? true);
        return new SolveResult(stopwatch.Elapsed, expected, actual, isCorrect, error);
    }

    private async Task<ResourceFiles> GetResourceFiles(ISolver solver)
    {
        var assembly = solver.GetAssembly();
        var workingDirectory = solver.GetWorkingDirectory();

        var input = await this.ReadResourceFile(assembly, Path.Combine(workingDirectory, "input.txt")).ConfigureAwait(false);
        var expectedPartOne = await this.ReadExpectedResourceFile(assembly, Path.Combine(workingDirectory, "expected1.txt")).ConfigureAwait(false);
        var expectedPartTwo = await this.ReadExpectedResourceFile(assembly, Path.Combine(workingDirectory, "expected2.txt")).ConfigureAwait(false);

        return new ResourceFiles(input, expectedPartOne, expectedPartTwo);
    }

    private async Task<string?> ReadExpectedResourceFile(Assembly assembly, string inputFile)
    {
        try
        {
            return await this.ReadResourceFile(assembly, inputFile).ConfigureAwait(false);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private async Task<string> ReadResourceFile(Assembly assembly, string filePath)
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
