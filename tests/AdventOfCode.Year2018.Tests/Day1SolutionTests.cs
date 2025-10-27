// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2018.Tests;

using AdventOfCode.Shared.Tests;
using System;

[TestClass]
public class Day1SolutionTests : SolverBaseTests<Day1Solution>
{
    [TestMethod]
    [DataRow("+1, -2, +3, +1", "3")]
    [DataRow("+1, +1, +1", "3")]
    [DataRow("+1, +1, -2", "0")]
    [DataRow("-1, -2, -3", "-6")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input.Replace(", ", Environment.NewLine))?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("+1, -2, +3, +1", "2")]
    [DataRow("+1, -1", "0")]
    [DataRow("+3, +3, +4, -2, -4", "10")]
    [DataRow("-6, +3, +8, +5, -6", "5")]
    [DataRow("+7, +7, -2, -7, -4", "14")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input.Replace(", ", Environment.NewLine))?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
