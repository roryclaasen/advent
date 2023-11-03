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

$ProjectName = "AdventOfCode.Year$Year"
$SolutionFile = Join-Path $PSScriptRoot "AdventOfCode.sln"
$SourceFolder = Join-Path $PSScriptRoot "source"
$TestsFolder = Join-Path $PSScriptRoot "tests"
$ProjectFolder = Join-Path $SourceFolder $ProjectName
$ProjectFile = Join-Path $ProjectFolder "$ProjectName.csproj"
$TestProjectFolder = Join-Path $TestsFolder "$ProjectName.Tests"
$TestProjectFile = Join-Path $TestProjectFolder "$ProjectName.Tests.csproj"

$DayFolder = Join-Path $ProjectFolder "Day$Day"
$CsFile = Join-Path $DayFolder "Day${Day}Solution.cs"
$TestCsFile = Join-Path $TestProjectFolder "Day${Day}SolutionTests.cs"

Write-Host "Creating day $Day for year $Year"

if (-not (Test-Path $ProjectFolder)) {
    dotnet new classlib -o $ProjectFolder --no-restore
    dotnet sln $SolutionFile add $ProjectFolder --solution-folder "Events"
    Remove-Item (Join-Path $ProjectFolder "Class1.cs")

    Set-Content -Path $ProjectFile -Value @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <EmbeddedResource Include="Day*\*.txt" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\AdventOfCode.Shared\AdventOfCode.Shared.csproj" />
  </ItemGroup>
</Project>
"@
}

if (-not (Test-path DayFolder)) {
    New-Item -Path $DayFolder -ItemType Directory
    New-Item -Path (Join-Path $DayFolder "input.txt") -ItemType File
    New-Item -Path (Join-Path $DayFolder "expected1.txt") -ItemType File
    New-Item -Path (Join-Path $DayFolder "expected2.txt") -ItemType File

    Set-Content -Path $CsFile -Value @"
namespace AdventOfCode.Year${Year};

using AdventOfCode.Shared;

[Problem(${Year}, ${Day}, null)]
public class Day${Day}Solution : ISolver
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

    Remove-Item (Join-Path $TestProjectFolder "GlobalUsings.cs")
    Remove-Item (Join-Path $TestProjectFolder "UnitTest1.cs")

    Set-Content -Path $TestProjectFile -Value @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\source\$ProjectName\$ProjectName.csproj" />
    <ProjectReference Include="..\AdventOfCode.Shared.Tests\AdventOfCode.Shared.Tests.csproj" />
  </ItemGroup>
</Project>
"@
}

if (-not (Test-Path $TestCsFile)) {
    Set-Content -Path $TestCsFile -Value @"
namespace AdventOfCode.Year$Year.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day${Day}SolutionTests : SolverBaseTests<Day${Day}Solution>
{
    [TestMethod]
    [DataRow("", "")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("", "")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
"@
}

dotnet restore $SolutionFile
