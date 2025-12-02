param (
    [Parameter(Mandatory = $true)]
    [int]$Year,

    [Parameter(Mandatory = $true)]
    [int]$Day
)

if ($Year -lt 2015) {
    throw "Year must be 2015 or greater"
}

if ($Day -lt 1 -or $Day -gt 25) {
    throw "Day must be between 1 and 25"
}

# Paths

$SessionFile = Join-Path $PSScriptRoot "session.txt"

$SourceFolder = Join-Path $PSScriptRoot "source"
$TestsFolder = Join-Path $PSScriptRoot "tests"
$SolutionFile = Join-Path $PSScriptRoot "AdventOfCode.slnx"

$ProjectName = "AdventOfCode.Year$Year"
$ProjectFolder = Join-Path $SourceFolder $ProjectName
$ProjectFile = Join-Path $ProjectFolder "$ProjectName.csproj"
$TestProjectFolder = Join-Path $TestsFolder "$ProjectName.Tests"
$TestProjectFile = Join-Path $TestProjectFolder "$ProjectName.Tests.csproj"

$DayFolder = Join-Path $ProjectFolder "Day$Day"
$CsFile = Join-Path $DayFolder "Day${Day}Solution.cs"
$TestCsFile = Join-Path $TestProjectFolder "Day${Day}SolutionTests.cs"

# Functions

function Get-SessionCookie {
    $cookie = Get-Content $SessionFile
    if (-not $cookie) {
        throw "Could not find session.txt"
    }

    return $cookie
}

function Get-DayInput {
    $cookie = Get-SessionCookie
    $content = Invoke-WebRequest "https://adventofcode.com/$Year/day/$Day/input" -Headers @{ Cookie = "session=$cookie" }
    if ($content.StatusCode -ne 200) {
        throw "Could not find input for day $Day for year $Year"
    }

    return $content.Content.TrimEnd("`n")
}

function Get-DayTitle {
    $content = Invoke-WebRequest "https://adventofcode.com/$Year/day/$Day"
    if ($content.StatusCode -ne 200) {
        throw "Could not find day $Day for year $Year"
    }

    $titleRegex = '<article class="day-desc"><h2>--- Day \d+: (.+) ---<\/h2>'

    $title = $content.Content | Select-String -Pattern $titleRegex | ForEach-Object { $_.Matches.Groups[1].Value }
    if (-not $title) {
        throw "Could not find title for day $Day for year $Year"
    }

    return $title
}

# Main
Write-Host "Creating day $Day for year $Year"

if (-not (Test-Path $ProjectFolder)) {
    dotnet new classlib -o $ProjectFolder --no-restore
    dotnet sln $SolutionFile add $ProjectFolder --solution-folder "Events"
    Remove-Item (Join-Path $ProjectFolder "Class1.cs")

    Set-Content -Path $ProjectFile -Value @"
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <AdditionalFiles Include="Day*\*.txt" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\AdventOfCode.Problem\AdventOfCode.Problem.csproj" OutputItemType="Analyzer" />
    <ProjectReference Include="..\AdventOfCode.Analyzers\AdventOfCode.Analyzers.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
    <ProjectReference Include="..\AdventOfCode.Shared\AdventOfCode.Shared.csproj" />
  </ItemGroup>
</Project>
"@

    write-host "You still need to add the project to the core project"
}

if (-not (Test-path DayFolder)) {
    New-Item -Path $DayFolder -ItemType Directory -ErrorAction SilentlyContinue
    New-Item -Path (Join-Path $DayFolder "Input.txt") -ItemType File
    New-Item -Path (Join-Path $DayFolder "Expected1.txt") -ItemType File
    New-Item -Path (Join-Path $DayFolder "Expected2.txt") -ItemType File

    try {
        $dayInput = Get-DayInput
        Set-Content -Path (Join-Path $DayFolder "Input.txt") -Value $dayInput -NoNewline
    }
    catch {
        Write-Warning "Could not find input for day $Day for year $Year"
    }

    function Get-DayTitleOrNull {
        try {
            $dayTitle = Get-DayTitle
            return "`"${dayTitle}`""
        }
        catch {
            Write-Warning "Could not find title for day $Day for year $Year"
            return "null"
        }
    }

    $title = Get-DayTitleOrNull

    Set-Content -Path $CsFile -Value @"
// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year${Year};

using AdventOfCode.Problem;

[Problem(${Year}, ${Day}, ${title})]
public partial class Day${Day}Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        return null;
    }

    public object? PartTwo(string input)
    {
        return null;
    }
}
"@
}

if (-not (Test-Path $TestProjectFolder)) {
    dotnet new mstest -o $TestProjectFolder --no-restore
    dotnet sln $SolutionFile add $TestProjectFolder --solution-folder "Tests\Events"

    Remove-Item (Join-Path $TestProjectFolder "Test1.cs")

    Set-Content -Path $TestProjectFile -Value @"
<Project Sdk="MSTest.Sdk">
  <ItemGroup>
    <ProjectReference Include="..\..\source\$ProjectName\$ProjectName.csproj" />
    <ProjectReference Include="..\AdventOfCode.Shared.Tests\AdventOfCode.Shared.Tests.csproj" />
  </ItemGroup>
</Project>
"@
}

if (-not (Test-Path $TestCsFile)) {
    Set-Content -Path $TestCsFile -Value @"
// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year$Year.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day${Day}SolutionTests : SolverBaseTests<Day${Day}Solution>
{
    [TestMethod]
    [DataRow("", "")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("", "")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
"@
}

dotnet restore $SolutionFile
