// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Tests;

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Cli.Commands;
using AdventOfCode.Cli.Finder;
using AdventOfCode.Cli.Runner;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Rocks;
using Spectre.Console;
using Spectre.Console.Rendering;

[TestClass]
public class SolutionRunnerTests
{
    private SolutionRunner runner;

    [TestInitialize]
    public void Initialize()
    {
        var console = new IAnsiConsoleCreateExpectations();
        var profile = new Profile(new AnsiConsoleOutput(TextWriter.Null), new Capabilities(), Encoding.UTF8);
        console.Setups.Profile.Gets().ReturnValue(profile);
        console.Setups.Write(Arg.Any<IRenderable>());

        var renderer = new SolutionResultRenderer(console.Instance(), new AdventUri(TimeProvider.System));

        this.runner = new SolutionRunner(renderer);
    }

    [TestMethod]
    public void RunAllStopsAfterCurrentPart()
    {
        using var cancellation = new CancellationTokenSource();

        var attempts = new List<string>();
        var solver = new TestSolver(() =>
        {
            attempts.Add("part 1 started");
            cancellation.Cancel();
            attempts.Add("part 1 finished");
        }, () => attempts.Add("part 2"));
        var nextSolver = new TestSolver(() => attempts.Add("next puzzle"), () => attempts.Add("next part"));

        var exception = Assert.ThrowsExactly<OperationCanceledException>(() => this.runner.RunAll([solver, nextSolver], cancellation.Token));

        Assert.AreEqual(cancellation.Token, exception.CancellationToken);
        Assert.AreSequenceEqual(["part 1 started", "part 1 finished"], attempts);
    }

    [TestMethod]
    public void RunAllStopsAfterPartTwoBeforeNextPuzzle()
    {
        using var cancellation = new CancellationTokenSource();

        var attempts = new List<string>();
        var solver = new TestSolver(() => attempts.Add("part 1"), () =>
        {
            cancellation.Cancel();
            attempts.Add("part 2 finished");
        });
        var nextSolver = new TestSolver(() => attempts.Add("next puzzle"), () => attempts.Add("next part"));

        Assert.ThrowsExactly<OperationCanceledException>(() => this.runner.RunAll([solver, nextSolver], cancellation.Token));
        Assert.AreSequenceEqual(["part 1", "part 2 finished"], attempts);
    }

    [TestMethod]
    public void RunAllRunsBothPartsWithoutCancellation()
    {
        var attempts = new List<string>();
        var solver = new TestSolver(() => attempts.Add("part 1"), () => attempts.Add("part 2"));

        var results = this.runner.RunAll([solver], CancellationToken.None);

        Assert.HasCount(1, results);
        Assert.AreSequenceEqual(["part 1", "part 2"], attempts);
    }

    [TestMethod]
    public async Task InvokeAsyncPassesCancellationToSolutionRunner()
    {
        using var cancellation = new CancellationTokenSource();

        var partOneFinished = false;
        var solver = new TestSolver(() =>
        {
            cancellation.Cancel();
            partOneFinished = true;
        }, () => Assert.Fail("Part 2 must not start."));

        var command = new AllCommand(new SolutionFinder([solver]), this.runner);

        var exception = await Assert.ThrowsExactlyAsync<OperationCanceledException>(() =>
            command.Parse([]).InvokeAsync(new InvocationConfiguration
            {
                EnableDefaultExceptionHandler = false,
                ProcessTerminationTimeout = Timeout.InfiniteTimeSpan
            }, cancellation.Token));

        Assert.IsTrue(partOneFinished);
        Assert.IsTrue(exception.CancellationToken.IsCancellationRequested);
    }

    private sealed class TestSolver(Action partOne, Action partTwo) : IProblemSolver, IProblemDetails, IProblemInput
    {
        public int Year => 2024;

        public int Day => 1;

        public string Name => "Cancellation test";

        public string Input => "input";

        public object? PartOne(string input)
        {
            partOne.Invoke();
            return 1;
        }

        public object? PartTwo(string input)
        {
            partTwo.Invoke();
            return 2;
        }
    }
}
