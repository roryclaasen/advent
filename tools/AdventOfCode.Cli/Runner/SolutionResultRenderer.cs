// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;
using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Rendering;

internal sealed class SolutionResultRenderer(IAnsiConsole console, AdventUri adventUri)
{
    private const int MaxAnswerLength = 60;

    public void Banner()
    {
        console.Write(new FigletText("Advent of Code")
            .Color(Color.Green)
            .LeftJustified());
        console.WriteLine();
    }

    public void Year(int year)
    {
        console.Write(new Rule($"[bold yellow][link={adventUri.Build(year)}]{year}[/][/]")
        {
            Style = new Style(Color.Olive),
            Justification = Justify.Center,
        });
        console.WriteLine();
    }

    public SolutionResult Run(IProblemSolver solver, Func<StatusContext, SolutionResult> work)
        => console.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(new Style(Color.Cyan1))
            .Start($"Solving [bold]{Markup.Escape(solver.GetDisplayName())}[/]...", work);

    public void Result(IProblemSolver solver, SolutionResult result)
    {
        var grid = new Grid()
            .AddColumn(new GridColumn().PadRight(2))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(2))
            .AddColumn(new GridColumn().RightAligned());

        grid.AddRow(BuildPartRow(1, result.Part1));
        grid.AddRow(BuildPartRow(2, result.Part2));

        var headerUri = adventUri.Build(solver.Year, solver.Day);
        var totalElapsed = result.Part1.Elapsed + result.Part2.Elapsed;

        console.Write(new Panel(new Padder(grid, new Padding(1, 0)))
        {
            Border = BoxBorder.Rounded,
            BorderStyle = BorderStyleFor(result),
            Expand = false,
            Header = new PanelHeader($" :calendar:  [bold][link={headerUri}]{Markup.Escape(solver.GetDisplayName())}[/][/] ", Justify.Left),
            Padding = new Padding(0, 0)
        });
        console.MarkupLine($"  [grey]total[/] {FormatElapsed(totalElapsed)}");
        console.WriteLine();

        if (result.Part1.IsError)
        {
            this.WriteException(result.Part1.Error);
        }

        if (result.Part2.IsError)
        {
            this.WriteException(result.Part2.Error);
        }
    }

    public void Summary(IReadOnlyList<SolutionResult> results)
    {
        if (results.Count <= 1)
        {
            return;
        }

        var pass = 0;
        var fail = 0;
        var unknown = 0;
        var error = 0;
        var total = TimeSpan.Zero;

        foreach (var part in results.SelectMany(r => r.GetParts()))
        {
            switch (part.Verdict)
            {
                case Verdict.Pass: pass++; break;
                case Verdict.Fail: fail++; break;
                case Verdict.Unknown: unknown++; break;
                case Verdict.Error: error++; break;
            }

            total += part.Elapsed;
        }

        console.Write(new Rule("[bold]Run summary[/]")
        {
            Style = new Style(Color.Grey),
            Justification = Justify.Center,
        });

        var grid = new Grid()
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn());

        grid.AddRow(
            $"[bold]{results.Count}[/] [grey]solutions[/]",
            $"[green]✓ {pass} pass[/]",
            $"[red]✗ {fail} fail[/]",
            $"[yellow]? {unknown} unknown[/]",
            $"[red bold]! {error} error[/]",
            $"[grey]total[/] {FormatElapsed(total)}");

        console.Write(new Padder(grid, new Padding(2, 1)));
    }

    public void NoSolutions() => console.MarkupLine("[yellow]No matching solutions found.[/]");

    private static IRenderable[] BuildPartRow(int part, PartResult result)
    {
        var verdict = result.Verdict;
        var (icon, color) = verdict switch
        {
            Verdict.Pass => ("✓", "green"),
            Verdict.Fail => ("✗", "red"),
            Verdict.Unknown => ("?", "yellow"),
            Verdict.Error => ("!", "red bold"),
            _ => ("?", "grey"),
        };

        var label = new Markup($"[{color}]{icon}[/]  [bold]Part {part}[/]");

        Markup answer;
        Markup detail;

        if (verdict == Verdict.Error)
        {
            answer = new Markup("[red bold]error[/]");
            detail = new Markup("[grey italic]see exception below[/]");
        }
        else
        {
            var actualText = string.IsNullOrWhiteSpace(result.Actual) ? "null" : Truncate(result.Actual);
            answer = new Markup($"[{color}]{Markup.Escape(actualText)}[/]");

            detail = verdict switch
            {
                Verdict.Pass => new Markup("[green]matches expected[/]"),
                Verdict.Unknown => new Markup("[grey italic]no expected answer[/]"),
                Verdict.Fail => new Markup($"[red]≠[/] [grey]{Markup.Escape(Truncate(result.Expected ?? string.Empty))}[/]"),
                _ => new Markup(string.Empty),
            };
        }

        return [label, answer, detail, new Markup("[grey]·[/]"), new Markup(FormatElapsed(result.Elapsed))];
    }

    private static Style BorderStyleFor(SolutionResult result)
    {
        var v1 = result.Part1.Verdict;
        var v2 = result.Part2.Verdict;

        if (v1 == Verdict.Error || v2 == Verdict.Error || v1 == Verdict.Fail || v2 == Verdict.Fail)
        {
            return new Style(Color.Red);
        }

        if (v1 == Verdict.Unknown || v2 == Verdict.Unknown)
        {
            return new Style(Color.Yellow);
        }

        return new Style(Color.Green);
    }

    private static string FormatElapsed(TimeSpan elapsed)
    {
        var ms = elapsed.TotalMilliseconds;
        var color = ms switch
        {
            < 500 => "green",
            < 2_000 => "yellow",
            < 5_000 => "orange1",
            _ => "red",
        };

        var text = ms switch
        {
            < 1 => $"{ms:F2} ms",
            < 10 => $"{ms:F2} ms",
            < 1_000 => $"{ms:F0} ms",
            < 60_000 => $"{elapsed.TotalSeconds:F2} s",
            _ => $"{(int)elapsed.TotalMinutes}m {elapsed.Seconds:D2}s",
        };

        return $"[{color}]{text}[/]";
    }

    private static string Truncate(string value)
        => value.Length <= MaxAnswerLength ? value : string.Concat(value.AsSpan(0, MaxAnswerLength - 1), "…");

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.")]
    private void WriteException(Exception exception) => console.WriteException(exception);
}
