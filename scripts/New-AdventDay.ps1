<#
.SYNOPSIS
    Scaffolds a new Advent of Code day (and year project, if needed).

.DESCRIPTION
    Creates the year source/test projects (if they don't already exist),
    then creates the day solution file, input file, expected answer files,
    and matching test class.

    If an Advent of Code session cookie is provided (either via -AocSession
    or the AOC_SESSION environment variable), the puzzle title and puzzle
    input are downloaded from https://adventofcode.com.

.PARAMETER Year
    Advent of Code year (e.g. 2024).

.PARAMETER Day
    Advent of Code day (1-25). If omitted, only the year scaffolding is created.

.PARAMETER AocSession
    Optional Advent of Code session cookie. Falls back to $env:AOC_SESSION.

.EXAMPLE
    ./scripts/New-AdventDay.ps1 -Year 2024 -Day 1

.EXAMPLE
    $env:AOC_SESSION = '...'; ./scripts/New-AdventDay.ps1 -Year 2024 -Day 5
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateRange(2015, 9999)]
    [int]$Year,

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 25)]
    [int]$Day,

    [Parameter(Mandatory = $false)]
    [string]$AocSession = $env:AOC_SESSION
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = 'Stop'

$RepoRoot   = Split-Path -Parent $PSScriptRoot
$Solution   = Join-Path $RepoRoot 'AdventOfCode.slnx'
$SourceRoot = Join-Path $RepoRoot 'source'
$TestsRoot  = Join-Path $RepoRoot 'tests'

$YearSourceFolder  = Join-Path $SourceRoot "AdventOfCode.Year$Year"
$YearSourceProject = Join-Path $YearSourceFolder "AdventOfCode.Year$Year.csproj"
$YearTestsFolder   = Join-Path $TestsRoot  "AdventOfCode.Year$Year.Tests"
$YearTestsProject  = Join-Path $YearTestsFolder  "AdventOfCode.Year$Year.Tests.csproj"

function Write-Info($message)    { Write-Host "[info]  $message" -ForegroundColor Cyan }
function Write-Skip($message)    { Write-Host "[skip]  $message" -ForegroundColor DarkGray }
function Write-Created($message) { Write-Host "[new]   $message" -ForegroundColor Green }

function Invoke-DotNet {
    & dotnet @args
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($args -join ' ') failed with exit code $LASTEXITCODE"
    }
}

function Write-FileIfMissing {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)][AllowEmptyString()][string]$Content,
        [switch]$Force,
        [switch]$NoTrailingNewline
    )

    if ((Test-Path -LiteralPath $Path) -and -not $Force) {
        Write-Skip (Split-Path -Leaf $Path)
        return
    }

    $dir = Split-Path -Parent $Path
    if (-not (Test-Path -LiteralPath $dir)) {
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
    }

    if ($NoTrailingNewline) {
        [System.IO.File]::WriteAllText($Path, $Content, [System.Text.UTF8Encoding]::new($false))
    } else {
        Set-Content -LiteralPath $Path -Value $Content -Encoding utf8
    }
    Write-Created (Split-Path -Leaf $Path)
}

function New-ProjectIfMissing {
    param(
        [Parameter(Mandatory)][string]$Folder,
        [Parameter(Mandatory)][string]$Project,
        [Parameter(Mandatory)][string]$Template,
        [Parameter(Mandatory)][string]$SolutionFolder,
        [Parameter(Mandatory)][string]$ProjectContents
    )

    if ((Test-Path -LiteralPath $Folder) -and (Test-Path -LiteralPath $Project)) {
        Write-Skip (Split-Path -Leaf $Project)
        return
    }

    Write-Info "Creating project $(Split-Path -Leaf $Project)"
    Invoke-DotNet new $Template -o $Folder --no-restore

    foreach ($boilerplate in @('Class1.cs', 'Test1.cs', 'UnitTest1.cs', 'MSTestSettings.cs')) {
        $path = Join-Path $Folder $boilerplate
        if (Test-Path -LiteralPath $path) {
            Remove-Item -LiteralPath $path -Force
        }
    }

    Write-FileIfMissing -Path $Project -Content $ProjectContents -Force
    Invoke-DotNet sln $Solution add $Folder --solution-folder $SolutionFolder
}

function New-YearSourceProject {
    $contents = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <IsYearSolver>true</IsYearSolver>
  </PropertyGroup>
</Project>
"@
    New-ProjectIfMissing `
        -Folder $YearSourceFolder `
        -Project $YearSourceProject `
        -Template 'classlib' `
        -SolutionFolder 'Source' `
        -ProjectContents $contents
}

function New-YearTestsProject {
    $contents = @"
<Project Sdk="MSTest.Sdk">
  <ItemGroup>
    <ProjectReference Include="..\..\source\AdventOfCode.Year$Year\AdventOfCode.Year$Year.csproj" />
    <ProjectReference Include="..\AdventOfCode.Shared.Tests\AdventOfCode.Shared.Tests.csproj" />
  </ItemGroup>
</Project>
"@
    New-ProjectIfMissing `
        -Folder $YearTestsFolder `
        -Project $YearTestsProject `
        -Template 'mstest' `
        -SolutionFolder 'Tests' `
        -ProjectContents $contents
}

function Get-DayNameFromWebsite {
    $url = "https://adventofcode.com/$Year/day/$Day"
    Write-Info "Fetching day name from $url"
    try {
        $response = Invoke-WebRequest -Uri $url -UseBasicParsing -ErrorAction Stop
        $match = [regex]::Match($response.Content, '<article class="day-desc"><h2>--- Day \d+: (.+) ---</h2>')
        if ($match.Success) { return $match.Groups[1].Value }
    } catch {
        Write-Warning "Could not fetch day name: $_"
    }
    Write-Warning 'Could not determine day name from website.'
    return $null
}

function Get-DayInputFromWebsite {
    if ([string]::IsNullOrWhiteSpace($AocSession)) {
        Write-Warning 'Skipping fetching input as AOC session is not provided.'
        return $null
    }

    $url = "https://adventofcode.com/$Year/day/$Day/input"
    Write-Info "Fetching day input from $url"
    try {
        $session = [Microsoft.PowerShell.Commands.WebRequestSession]::new()
        $cookie  = [System.Net.Cookie]::new('session', $AocSession, '/', '.adventofcode.com')
        $session.Cookies.Add($cookie)

        $response = Invoke-WebRequest -Uri $url -WebSession $session -UseBasicParsing -ErrorAction Stop
        $body = $response.Content
        if ([string]::IsNullOrWhiteSpace($body) -or $body -match '404 Not Found' -or $body -match '<title>500 Internal Server Error</title>') {
            Write-Warning 'Could not fetch input from website. Please ensure the AOC session is correct and has access to this day''s input.'
            return $null
        }
        return $body
    } catch {
        Write-Warning "Could not fetch input: $_"
        return $null
    }
}

function New-DaySource {
    $dayFolder   = Join-Path $YearSourceFolder "Day$Day"
    $solutionCs  = Join-Path $dayFolder "Day${Day}Solution.cs"
    $inputTxt    = Join-Path $dayFolder 'Input.txt'
    $expected1   = Join-Path $dayFolder 'Expected1.txt'
    $expected2   = Join-Path $dayFolder 'Expected2.txt'

    if (-not (Test-Path -LiteralPath $dayFolder)) {
        New-Item -ItemType Directory -Path $dayFolder -Force | Out-Null
    }

    Write-FileIfMissing -Path $expected1 -Content ''
    Write-FileIfMissing -Path $expected2 -Content ''

    if (-not (Test-Path -LiteralPath $solutionCs)) {
        $name = Get-DayNameFromWebsite
        $nameLiteral = if ([string]::IsNullOrWhiteSpace($name)) { 'null' } else { "`"$name`"" }

        $solutionContents = @"
// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year$Year;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem($Year, $Day, $nameLiteral)]
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
        Write-FileIfMissing -Path $solutionCs -Content $solutionContents
    } else {
        Write-Skip (Split-Path -Leaf $solutionCs)
    }

    if (-not (Test-Path -LiteralPath $inputTxt)) {
        $inputData = Get-DayInputFromWebsite
        $text = if ($null -eq $inputData) { '' } else { $inputData.TrimEnd("`r", "`n") }
        Write-FileIfMissing -Path $inputTxt -Content $text -NoTrailingNewline
    } else {
        Write-Skip (Split-Path -Leaf $inputTxt)
    }
}

function New-DayTests {
    $testFile = Join-Path $YearTestsFolder "Day${Day}SolutionTests.cs"

    if (Test-Path -LiteralPath $testFile) {
        Write-Skip (Split-Path -Leaf $testFile)
        return
    }

    $contents = @"
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
    Write-FileIfMissing -Path $testFile -Content $contents
}

Write-Info "Scaffolding Advent of Code $Year$(if ($PSBoundParameters.ContainsKey('Day')) { " day $Day" })"

Invoke-DotNet restore $Solution

New-YearSourceProject
New-YearTestsProject

if ($PSBoundParameters.ContainsKey('Day')) {
    New-DaySource
    New-DayTests
}

Write-Info 'Done.'
