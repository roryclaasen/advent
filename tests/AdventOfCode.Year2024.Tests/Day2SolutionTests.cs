// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day2SolutionTests : SolverBaseTests<Day2Solution>
{
    [TestMethod]
    [DataRow("7 6 4 2 1", "1")]
    [DataRow("1 2 7 8 9", "0")]
    [DataRow("9 7 6 2 1", "0")]
    [DataRow("1 3 2 4 5", "0")]
    [DataRow("8 6 4 4 1", "0")]
    [DataRow("1 3 6 7 9", "1")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("7 6 4 2 1", "1")]
    [DataRow("1 2 7 8 9", "0")]
    [DataRow("9 7 6 2 1", "0")]
    [DataRow("1 3 2 4 5", "1")]
    [DataRow("8 6 4 4 1", "1")]
    [DataRow("1 3 6 7 9", "1")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
