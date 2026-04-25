---
agent: 'agent'
description: Scaffold a new Advent of Code problem (year/day) by running scripts/New-AdventDay.ps1
---

# Generate a new Advent of Code problem

Your task is to scaffold a new Advent of Code problem by running the repository's
PowerShell scaffolding script: `scripts/New-AdventDay.ps1`.

## Gathering inputs

Extract the year and (optionally) the day from the user's request.

- **Year** (required): a 4-digit year between `2015` and `9999`.
- **Day** (optional): an integer between `1` and `25`.

If the user did not provide a year, ask for one before proceeding. Do **not**
invent or guess values. The day is optional — if omitted, only the year
project (and its test project) will be scaffolded.

## Running the script

From the repository root, invoke the script with `pwsh`:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./scripts/New-AdventDay.ps1 -Year <YEAR> [-Day <DAY>]
```

- If `$env:AOC_SESSION` is set, the script will automatically fetch the puzzle
  title and input from adventofcode.com. Do not ask the user for this value —
  the script handles the fallback itself.
- The script is idempotent: every action reports `[new]`, `[skip]`, or
  `[info]`. Do not pre-check whether files exist; just run the script and let
  it report.

## What the script does

When it creates a **new year project** it will also:

1. Create `source/AdventOfCode.Year{YYYY}` (classlib) and
   `tests/AdventOfCode.Year{YYYY}.Tests` (mstest), adding both to
   `AdventOfCode.slnx`.

When a `-Day` is also provided, it scaffolds `DayNSolution.cs`,
`DayNSolutionTests.cs`, `Input.txt`, `Expected1.txt`, and `Expected2.txt`
under the corresponding `DayN/` folder.

The script intentionally does **not** edit the CLI project, `Program.cs`,
or `launchSettings.json` — you (Copilot) own those steps. See below.

## After running the script: wire the year into the CLI

If the script created a **new year project** (i.e. this is the first day
scaffolded for `{YYYY}`), perform the following edits yourself. Skip any
step that is already done (the user may have re-run the script). All four
edits must end up sorted by year ascending, with `{YYYY}` slotted into the
correct position relative to the existing entries.

1. **`tools/AdventOfCode.Cli/AdventOfCode.Cli.csproj`** — add a
   `<ProjectReference>` to the new year project inside the existing
   year-references `<ItemGroup>`:

   ```xml
   <ProjectReference Include="..\..\source\AdventOfCode.Year{YYYY}\AdventOfCode.Year{YYYY}.csproj" />
   ```

2. **`tools/AdventOfCode.Cli/Program.cs`** — add a `using` directive in the
   sorted `using AdventOfCode.Year####;` block at the top of the file:

   ```csharp
   using AdventOfCode.Year{YYYY};
   ```

3. **`tools/AdventOfCode.Cli/Program.cs`** — register the year's solutions
   in the sorted `builder.Services.AddSolutionsFor####();` block inside
   `BuildApplication`:

   ```csharp
       builder.Services.AddSolutionsFor{YYYY}();
   ```

   `AddSolutionsFor{YYYY}` is generated automatically by the
   `ProblemGenerator` source generator from any `[Problem({YYYY}, …)]`
   class — do not try to define it manually. If your IDE flags it as
   missing before the first build, run `dotnet build` once to materialise
   the generated source.

4. **`tools/AdventOfCode.Cli/Properties/launchSettings.json`** — add a
   profile in the sorted year block:

   ```json
       "AdventOfCode.Cli ({YYYY})": {
         "commandName": "Project",
         "commandLineArgs": "last --year {YYYY}"
       },
   ```

   Make sure the trailing comma matches the surrounding entries (no
   trailing comma if it becomes the last profile in its block).

For an existing year (just adding a new day), none of the four CLI edits
are needed — the year is already wired up.

## Verify the build

After the script and any CLI edits, run a build from the repo root to
confirm everything compiles and the source generators have produced
`AddSolutionsFor{YYYY}` etc.:

```powershell
dotnet build AdventOfCode.slnx --configuration Release
```

If the build fails, fix the issue (most commonly: a missing CLI edit,
the wrong sort position, or a stray comma in `launchSettings.json`) and
build again until it succeeds. Do not report success until the build is
green.

## After running

1. Summarise what the script output (which files were `[new]` vs `[skip]`),
   which CLI files you edited (or "no CLI changes – existing year"), and
   confirm the build succeeded.
2. Do **not** commit the changes — leave them staged/unstaged for the user
   to review.
3. If the script failed, show the error and stop. Do not attempt to work
   around it by manually creating files.
