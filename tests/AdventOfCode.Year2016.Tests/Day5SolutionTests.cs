// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day5SolutionTests : SolverBaseTests<Day5Solution>
{
    [TestMethod]
    [Ignore("Test takes too long to run")]
    [DataRow("abc", "18f47a30")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [Ignore("Test takes too long to run")]
    [DataRow("abc", "05ace8e3")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
