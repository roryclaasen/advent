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
2. Add a `ProjectReference` from `tools/AdventOfCode.Cli` to the new year
   project.
3. Insert `using AdventOfCode.Year{YYYY};` and
   `builder.Services.AddSolutionsFor{YYYY}();` into
   `tools/AdventOfCode.Cli/Program.cs` (in sorted position).
4. Add a matching `"AdventOfCode.Cli ({YYYY})"` profile to
   `tools/AdventOfCode.Cli/Properties/launchSettings.json`.

When a `-Day` is also provided, it scaffolds `DayNSolution.cs`,
`DayNSolutionTests.cs`, `Input.txt`, `Expected1.txt`, and `Expected2.txt`
under the corresponding `DayN/` folder.

## After running

1. Summarise what the script output (which files were `[new]` vs `[skip]`).
2. Do **not** commit the changes — leave them staged/unstaged for the user to
   review.
3. If the script failed, show the error and stop. Do not attempt to work
   around it by manually creating files.
