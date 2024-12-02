namespace AdventOfCode.Infrastructure;

using AdventOfCode.Shared;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal sealed partial class Runner
{
    [GeneratedRegex("\\r\\n|\\n\\r|\\n|\\r")]
    private partial Regex NewLineRegex();

    private async Task<ResourceFiles> GetResourceFiles(ISolverWithDetails solver)
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
            var file = await this.ReadResourceFile(assembly, inputFile).ConfigureAwait(false);
            return string.IsNullOrWhiteSpace(file) ? null : file;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private async Task<string> ReadResourceFile(Assembly assembly, string filePath)
    {
        var input = await assembly.ReadResourceFile(filePath).ConfigureAwait(false);
        var normalized = this.NewLineRegex().Replace(input, Environment.NewLine);
        if (normalized.EndsWith(Environment.NewLine, StringComparison.Ordinal))
        {
            normalized = normalized[..^Environment.NewLine.Length];
        }

        return normalized;
    }

    private record ResourceFiles(string Input, string? ExpectedPartOne, string? ExpectedPartTwo);
}
