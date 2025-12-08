// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Serilog;

[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines")]
internal partial interface ICreateDay : IRestore
{
    [Parameter("Year of the Advent of Code to create the day for")]
    public AocNumber Year => this.TryGetValue(() => this.Year);

    [Parameter("Day of the Advent of Code to create")]
    public AocNumber Day => this.TryGetValue(() => this.Day);

    [Parameter("AOC session cookie value for fetching input from the website")]
    [Secret]
    public string? AocSession
        => this.TryGetValue(() => this.AocSession)
        ?? Environment.GetEnvironmentVariable("AOC_SESSION")
        ?? null;

    public Target CreateYearSourceProject => _ => _
        .Unlisted()
        .Requires(() => this.Solution)
        .Requires(() => this.Year)
        .DependsOn(this.Restore)
        .Executes(this.RunCreateYearSourceProject);

    public Target CreateYearTestsProject => _ => _
        .Unlisted()
        .Requires(() => this.Solution)
        .Requires(() => this.Year)
        .DependsOn(this.Restore)
        .Executes(this.RunCreateYearTestsProject);

    public Target CreateDaySource => _ => _
        .Unlisted()
        .Requires(() => this.Solution)
        .Requires(() => this.Year)
        .Requires(() => this.Day)
        .DependsOn(this.CreateYearSourceProject)
        .Executes(this.RunCreateDaySource);

    public Target CreateDayTests => _ => _
        .Unlisted()
        .Requires(() => this.Solution)
        .Requires(() => this.Year)
        .Requires(() => this.Day)
        .DependsOn(this.CreateYearTestsProject)
        .Executes(this.RunCreateDayTests);

    public Target CreateDay => _ => _
        .DependsOn(this.CreateDaySource)
        .DependsOn(this.CreateDayTests);

    private AbsolutePath YearSourceFolder => this.Solution!.Directory / "source" / $"AdventOfCode.Year{this.Year.Value}";

    private AbsolutePath YearSourceProject => this.YearSourceFolder / $"AdventOfCode.Year{this.Year.Value}.csproj";

    private AbsolutePath YearTestsFolder => this.Solution!.Directory / "tests" / $"AdventOfCode.Year{this.Year.Value}.Tests";

    private AbsolutePath YearTestsProject => this.YearTestsFolder / $"AdventOfCode.Year{this.Year.Value}.Tests.csproj";

    [GeneratedRegex(@"<article class=""day-desc""><h2>--- Day \d+: (.+) ---</h2>")]
    private static partial Regex DayWebsiteRegex { get; }

    public void RunCreateYearSourceProject()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(this.Year?.Value ?? 0, nameof(this.Year));

        this.CreateNewProject(this.YearSourceFolder, this.YearSourceProject, "classlib", "Source", @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <IsYearSolver>true</IsYearSolver>
  </PropertyGroup>
</Project>");
    }

    public void RunCreateYearTestsProject()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(this.Year?.Value ?? 0, nameof(this.Year));

        this.CreateNewProject(this.YearTestsFolder, this.YearTestsProject, "mstest", "Tests", @$"<Project Sdk=""MSTest.Sdk"">
  <ItemGroup>
    <ProjectReference Include=""..\..\source\AdventOfCode.Year{this.Year!.Value}.Tests\AdventOfCode.Year{this.Year!.Value}.Tests.csproj"" />
    <ProjectReference Include=""..\AdventOfCode.Shared.Tests\AdventOfCode.Shared.Tests.csproj"" />
  </ItemGroup>
</Project>");
    }

    private async Task RunCreateDaySource()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(this.Year?.Value ?? 0, nameof(this.Year));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(this.Day?.Value ?? 0, nameof(this.Day));

        var sourceFolder = this.YearSourceFolder / $"Day{this.Day!.Value}";
        if (!sourceFolder.Exists())
        {
            sourceFolder.CreateDirectory();
        }

        WriteContentsToFile(sourceFolder / "Expected1.txt", string.Empty);
        WriteContentsToFile(sourceFolder / "Expected2.txt", string.Empty);

        var solutionFile = sourceFolder / $"Day{this.Day!.Value}Solution.cs";
        if (!solutionFile.FileExists())
        {
            var name = await this.GetDayNameFromWebsite();
            name = string.IsNullOrWhiteSpace(name) ? "null" : $"\"{name}\"";

            Log.Information($"Writing to file {solutionFile.Name}");
            solutionFile.WriteAllText(@$"// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year{this.Year!.Value};

using System;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Extensions;

[Problem({this.Year!.Value}, {this.Day!.Value}, {name})]
public partial class Day{this.Day!.Value}Solution : IProblemSolver
{{
    public object? PartOne(string input)
    {{
        return null;
    }}

    public object? PartTwo(string input)
    {{
        return null;
    }}
}}");
        }
        else
        {
            Log.Information($"File {solutionFile.Name} already exists, skipping creation.");
        }

        var inputFile = sourceFolder / "Input.txt";
        if (!inputFile.FileExists())
        {
            var input = await this.GetDayInputFromWebsite();

            Log.Information($"Writing to file {inputFile.Name}");
            inputFile.WriteAllText(input?.TrimEnd('\r', '\n') ?? string.Empty, eofLineBreak: false);
        }
        else
        {
            Log.Information($"File {inputFile.Name} already exists, skipping creation.");
        }
    }

    private void RunCreateDayTests()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(this.Year?.Value ?? 0, nameof(this.Year));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(this.Day?.Value ?? 0, nameof(this.Day));

        var testFile = this.YearTestsFolder / $"Day{this.Day!.Value}SolutionTests.cs";
        if (testFile.Exists())
        {
            Log.Information($"File {testFile.Name} already exists, skipping creation.");
            return;
        }

        Log.Information($"Writing to file {testFile.Name}");
        testFile.WriteAllText(@$"// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year{this.Year!.Value}.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day{this.Day.Value}SolutionTests : SolverBaseTests<Day{this.Day.Value}Solution>
{{
    [TestMethod]
    [DataRow("""", """")]
    public void SolvePart1(string input, string answer)
    {{
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }}

    [TestMethod]
    [DataRow("""", """")]
    public void SolvePart2(string input, string answer)
    {{
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }}
}}");
    }

    private async Task<string?> GetDayNameFromWebsite()
    {
        var url = $"https://adventofcode.com/{this.Year.Value}/day/{this.Day.Value}";

        Log.Information($"Fetching day name from {url}");
        var response = await HttpTasks.HttpDownloadStringAsync($"https://adventofcode.com/{this.Year.Value}/day/{this.Day.Value}");

        var match = DayWebsiteRegex.Match(response);
        var name = match.Success ? match.Groups[1].Value : null;
        if (string.IsNullOrWhiteSpace(name))
        {
            Log.Warning("Could not determine day name from website.");
        }

        return name;
    }

    private async Task<string?> GetDayInputFromWebsite()
    {
        if (string.IsNullOrWhiteSpace(this.AocSession))
        {
            Log.Warning("Skipping fetching input as AOC session is not provided.");
            return null;
        }

        var url = $"https://adventofcode.com/{this.Year.Value}/day/{this.Day.Value}/input";

        Log.Information($"Fetching day input from {url}");
        var response = await HttpTasks.HttpDownloadStringAsync(url, headerConfigurator: (headers) =>
        {
            headers.Add("Cookie", $"session={this.AocSession}");
        });

        var input = string.IsNullOrWhiteSpace(response)
            || response.Contains("404 Not Found")
            || response.Contains("<title>500 Internal Server Error</title>")
            ? null
            : response;

        if (input is null)
        {
            Log.Warning("Could not fetch input from website. Please ensure the AOC session is correct and has access to this day's input.");
        }

        return input;
    }

    private void CreateNewProject(AbsolutePath folder, AbsolutePath project, string projectType, string solutionFolder, string projectContents)
    {
        if (folder.DirectoryExists() && project.FileExists())
        {
            Log.Information($"Project {project.Name} already exists, skipping creation.");
            return;
        }

        DotNetTasks.DotNet($"new {projectType} -o {folder} --no-restore");
        (this.YearTestsFolder / "Class1.cs").DeleteFile();
        (this.YearTestsFolder / "Test1.cs").DeleteFile();
        WriteContentsToFile(project, projectContents, true);

        DotNetTasks.DotNet($"sln {this.Solution!} add {folder} --solution-folder {solutionFolder}");
    }

    private static void WriteContentsToFile(AbsolutePath file, string contents, bool forceWrite = false)
    {
        if (forceWrite || !file.FileExists())
        {
            Log.Information($"Writing to file {file.Name}");
            file.WriteAllText(contents);
        }
    }
}
