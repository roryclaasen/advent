// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Runner;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Problem;
using AdventOfCode.Problem.Extensions;
using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Rendering;

internal sealed class SolutionResultRenderer(IAnsiConsole console, AdventUri adventUri)
{
    private const int TickIntervalMs = 80;

    private enum PartStatus
    {
        Pending,
        Running,
        Done
    }

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

    public SolutionResult Run(IProblemSolver solver, Func<PartResult> solvePart1, Func<PartResult> solvePart2)
    {
        PartResult p1;
        PartResult p2;

        if (console.Profile.Capabilities.Interactive)
        {
            (p1, p2) = this.RunLive(solver, solvePart1, solvePart2);
        }
        else
        {
            p1 = solvePart1();
            p2 = solvePart2();
            console.Write(this.BuildPanel(solver, PartState.Done(p1), PartState.Done(p2)));
        }

        var total = p1.Elapsed + p2.Elapsed;
        console.MarkupLine($"  [grey]total[/] {FormatElapsed(total)}");
        console.WriteLine();

        if (p1.IsError)
        {
            this.WriteException(p1.Error);
        }

        if (p2.IsError)
        {
            this.WriteException(p2.Error);
        }

        return new SolutionResult(p1, p2);
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

    private static IRenderable[] BuildPartRow(int part, PartState state) => state.Status switch
    {
        PartStatus.Pending => BuildPendingRow(part),
        PartStatus.Running => BuildRunningRow(part, state),
        _ => BuildDoneRow(part, state.Result!.Value),
    };

    private static IRenderable[] BuildPendingRow(int part) => [
        new Markup($"[grey dim]·[/]  [grey dim]Part {part}[/]"),
        new Markup("[grey dim italic]queued[/]"),
        new Markup(string.Empty),
        new Markup(string.Empty),
        new Markup(string.Empty)
    ];

    private static IRenderable[] BuildRunningRow(int part, PartState state)
    {
        var spinner = SpinnerFrame(state.RunningElapsed);
        return [
            new Markup($"[cyan]{spinner}[/]  [bold]Part {part}[/]"),
            new Markup("[cyan italic]solving…[/]"),
            new Markup(string.Empty),
            new Markup("[grey]·[/]"),
            new Markup($"[cyan]{FormatElapsedText(state.RunningElapsed)}[/]")
        ];
    }

    private static IRenderable[] BuildDoneRow(int part, PartResult r)
    {
        var verdict = r.Verdict;
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
            var actualText = string.IsNullOrWhiteSpace(r.Actual) ? "null" : r.Actual;
            answer = new Markup($"[{color}]{Markup.Escape(actualText)}[/]");

            detail = verdict switch
            {
                Verdict.Pass => new Markup("[green]matches expected[/]"),
                Verdict.Unknown => new Markup("[grey italic]no expected answer[/]"),
                Verdict.Fail => new Markup($"[red]≠[/] [grey]{Markup.Escape(r.Expected ?? string.Empty)}[/]"),
                _ => new Markup(string.Empty),
            };
        }

        return [label, answer, detail, new Markup("[grey]·[/]"), new Markup(FormatElapsed(r.Elapsed))];
    }

    private static Style BorderStyleFor(PartState part1, PartState part2)
    {
        if (part1.Status != PartStatus.Done || part2.Status != PartStatus.Done)
        {
            return new Style(Color.Grey);
        }

        var v1 = part1.Result!.Value.Verdict;
        var v2 = part2.Result!.Value.Verdict;

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

    private static string SpinnerFrame(TimeSpan elapsed)
    {
        var frames = Spinner.Known.Dots.Frames;
        var idx = (int)(elapsed.TotalMilliseconds / TickIntervalMs) % frames.Count;
        if (idx < 0)
        {
            idx = 0;
        }

        return Markup.Escape(frames[idx]);
    }

    private static string FormatElapsedText(TimeSpan elapsed)
    {
        var ms = elapsed.TotalMilliseconds;
        return ms switch
        {
            < 1 => $"{ms:F2} ms",
            < 10 => $"{ms:F2} ms",
            < 1_000 => $"{ms:F0} ms",
            < 60_000 => $"{elapsed.TotalSeconds:F2} s",
            _ => $"{(int)elapsed.TotalMinutes}m {elapsed.Seconds:D2}s",
        };
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

        return $"[{color}]{FormatElapsedText(elapsed)}[/]";
    }

    private (PartResult P1, PartResult P2) RunLive(IProblemSolver solver, Func<PartResult> solvePart1, Func<PartResult> solvePart2)
    {
        PartResult p1 = default;
        PartResult p2 = default;

        console.Live(this.BuildPanel(solver, PartState.Pending(), PartState.Pending()))
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Bottom)
            .Start(ctx =>
            {
                p1 = this.RunPartLive(ctx, solver, solvePart1, isPart1: true, otherDone: null);
                p2 = this.RunPartLive(ctx, solver, solvePart2, isPart1: false, otherDone: p1);
                ctx.UpdateTarget(this.BuildPanel(solver, PartState.Done(p1), PartState.Done(p2)));
            });

        return (p1, p2);
    }

    private PartResult RunPartLive(LiveDisplayContext ctx, IProblemSolver solver, Func<PartResult> solve, bool isPart1, PartResult? otherDone)
    {
        var sw = Stopwatch.StartNew();
        var task = Task.Run(solve);

        while (!task.IsCompleted)
        {
            var running = PartState.Running(sw.Elapsed);
            var (s1, s2) = isPart1
                ? (running, PartState.Pending())
                : (PartState.Done(otherDone!.Value), running);

            ctx.UpdateTarget(this.BuildPanel(solver, s1, s2));
            Thread.Sleep(TickIntervalMs);
        }

        var result = task.GetAwaiter().GetResult();
        var (d1, d2) = isPart1
            ? (PartState.Done(result), PartState.Pending())
            : (PartState.Done(otherDone!.Value), PartState.Done(result));

        ctx.UpdateTarget(this.BuildPanel(solver, d1, d2));
        return result;
    }

    private Panel BuildPanel(IProblemSolver solver, PartState part1, PartState part2)
    {
        var grid = new Grid()
            .AddColumn(new GridColumn().PadRight(2))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(3))
            .AddColumn(new GridColumn().PadRight(2))
            .AddColumn(new GridColumn().RightAligned());

        grid.AddRow(BuildPartRow(1, part1));
        grid.AddRow(BuildPartRow(2, part2));

        var headerUri = adventUri.Build(solver.Year, solver.Day);
        return new Panel(new Padder(grid, new Padding(1, 0)))
        {
            Border = BoxBorder.Rounded,
            BorderStyle = BorderStyleFor(part1, part2),
            Expand = false,
            Header = new PanelHeader($"[bold][link={headerUri}]{Markup.Escape(solver.GetDisplayName())}[/][/]", Justify.Left),
            Padding = new Padding(0, 0)
        };
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.")]
    private void WriteException(Exception exception) => console.WriteException(exception);

    private readonly record struct PartState(PartStatus Status, TimeSpan RunningElapsed, PartResult? Result)
    {
        public static PartState Pending() => new(PartStatus.Pending, TimeSpan.Zero, null);

        public static PartState Running(TimeSpan elapsed) => new(PartStatus.Running, elapsed, null);

        public static PartState Done(PartResult result) => new(PartStatus.Done, TimeSpan.Zero, result);
    }
}
