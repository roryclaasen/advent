# Advent of Code – Agent Instructions

C# / .NET 11 solutions to [Advent of Code](https://adventofcode.com) puzzles.
Per-year class libraries, an MSTest test project per year, a `System.CommandLine`-based CLI runner, and Roslyn source generators that wire problems together.

## Build, test, lint

The solution file is `AdventOfCode.slnx` (XML solution format).
The repo pins SDKs in `global.json` (.NET, MSTest.Sdk); `dotnet test` runs via the new Microsoft.Testing.Platform runner.

- Restore / build / test the whole repo:
  - `dotnet restore AdventOfCode.slnx`
  - `dotnet build AdventOfCode.slnx --configuration Release --no-restore`
  - `dotnet test --solution AdventOfCode.slnx --configuration Release --no-restore --no-build`
- Build/test a single project: `dotnet test tests/AdventOfCode.Year2024.Tests/AdventOfCode.Year2024.Tests.csproj`
- Run a single test (Microsoft.Testing.Platform syntax, not VSTest): append `-- --filter-method "*Day1SolutionTests.SolvePart1*"` (or `--filter-class`). Plain `--filter "Name~..."` will not work.
- Publish (AOT-compatible; `IsAotCompatible=true` in `Directory.Build.props`): `dotnet publish AdventOfCode.slnx -c Release -r <rid>` where rid ∈ `win-x64`, `win-arm64`, `linux-x64`, `osx-x64`.
- Run a year/day via the CLI: `dotnet run --project tools/AdventOfCode.Cli -- last --year 2024` (subcommands: `list`, `all`, `today`, `last`).
- Lint: Roslynator.Analyzers, Roslynator.Formatting.Analyzers, and .NET analyzers run during `dotnet build` (`EnforceCodeStyleInBuild=true`). The repository has no separate lint command.

## Scaffolding new years/days

**When the user asks to add, create, or scaffold a new Advent of Code year or day, use the `new-problem` skill** in `.github/skills/new-problem/SKILL.md`.
Read and follow the skill before scaffolding or making related CLI edits.

## Architecture

- **`source/AdventOfCode.Problem`** – tiny abstractions (`IProblemSolver`, `IProblemDetails`, `IProblemInput`, `[Problem(year, day, name?)]`). Referenced by year projects as both a regular reference and as an `Analyzer` (so the attribute is visible to source generators).
- **`source/AdventOfCode.Analyzers`** – two Roslyn `IIncrementalGenerator`s referenced by every year project as `OutputItemType="Analyzer" ReferenceOutputAssembly="false"`:
  - `ProblemGenerator` finds every `[Problem]`-attributed `partial class` and emits a `Day{N}Solution.g.cs` implementing `IProblemDetails`, plus one aggregated `AddSolutionsFor{YYYY}()` `IServiceCollection` extension per year.
  - `ResourceGenerator` reads `<AdditionalFiles>` (the `Day*\*.txt` files) and emits a partial property per file (`Input`, `Expected1`, `Expected2`) onto the matching `Day{N}Solution` class, implementing `IProblemInput` etc. **This is why solution classes must be `partial` and why `Input.txt` / `Expected1.txt` / `Expected2.txt` must live in `DayN/`** — they are compile-time inputs, not runtime files.
- **`source/AdventOfCode.Year{YYYY}`** – per-year class library. Opt in to the per-year boilerplate by setting `<IsYearSolver>true</IsYearSolver>` in the `.csproj`; `source/Source.Build.props` then adds `AdditionalFiles Include="Day*\*.txt"` and the analyzer/Problem/Shared references. Each `Day{N}Solution` is a `public partial class` with `[Problem(YYYY, N, "Name")]` implementing `IProblemSolver`.
- **`source/AdventOfCode.Shared`** – cross-year helpers (`Direction`, `AdventUri`, extensions in `Extensions/`, numerics/memory/union helpers). Use these instead of reimplementing; e.g. `string.Lines()` in `Day1Solution.cs`.
- **`tools/AdventOfCode.Cli`** – `System.CommandLine` host. Wires solutions via the generator-produced `AddSolutionsFor{YYYY}()` extensions; `SolutionFinder` / `SolutionRunner` discover and execute them. `launchSettings.json` has one profile per year. New years/days are added via the scaffolder, not by hand.
- **`tests/AdventOfCode.Year{YYYY}.Tests`** – MSTest project using `Microsoft.Testing.Platform` (configured in `global.json` `"runner"`). Each test class derives from `SolverBaseTests<TSolver>` (in `AdventOfCode.Shared.Tests`) which `new`s the solver in `[TestInitialize]`. Tests use `[DataRow]` literals for the puzzle’s sample input/answer.

## Conventions

- All `.cs` files start with the copyright header defined by `file_header_template` in `.editorconfig`:
  ```
  // Copyright (c) Rory Claasen. All rights reserved.
  // Licensed under the MIT license. See LICENSE in the project root for license information.
  ```
- The repository uses a file-scoped `namespace` followed by `using` directives **inside** the namespace.
- Solution classes are `public partial class Day{N}Solution : IProblemSolver` — the `partial` is mandatory because generators add the `Year`/`Day`/`Name`/`Input`/`Expected1`/`Expected2` members.
- Year projects use `InternalsVisibleTo("$(AssemblyName).Tests")` (set in `Source.Build.props`), so tests may reach internals of the year being tested.
- Indentation: 4 spaces (2 for `*.{json,yml,slnx,csproj,props,config}`); CRLF; final newline; **except** `*.txt` (puzzle inputs) which keep no trailing newline and preserve trailing whitespace — the scaffolder writes inputs without a trailing newline on purpose, do not "fix" them.
- Central package management is on (`Directory.Packages.props`, `ManagePackageVersionsCentrally=true`); add `<PackageReference Include="X" />` without a version, and add the `<PackageVersion>` entry in `Directory.Packages.props`.
- Targets `net10.0`, `Nullable=enable`, `IsAotCompatible=true` — avoid reflection / dynamic / unbounded generics that break trimming/AOT in the year libraries.
- `InvariantGlobalization=true` in `Directory.Build.props` makes the CLI and test executables use invariant globalization. Puzzle parsing does not need explicit culture arguments.
- Test runner is Microsoft.Testing.Platform (not VSTest). Use `--filter-method` / `--filter-class` (with `*` wildcards), not `--filter`. Tests are parallelised at method scope (`tests/MSTestSettings.cs`).
- CI (`.github/workflows/dotnet.yml`) runs restore → build → test → publish per RID; mirror these commands locally before pushing.

## Git workflow

- **Do not run `git commit` (or `git push`) unless the user explicitly asks for it.** Stage and edit freely, but leave commit timing to the user. Phrases like "commit this", "commit it", or "push it" are the signal — without them, finish the work and stop.
