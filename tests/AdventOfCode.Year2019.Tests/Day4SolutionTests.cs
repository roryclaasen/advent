// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2019.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day4SolutionTests : SolverBaseTests<Day4Solution>
{
    [TestMethod]
    [DataRow("111111-111111", "1")]
    [DataRow("223450-223450", "0")]
    [DataRow("123789-123789", "0")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("112233-112233", "1")]
    [DataRow("123444-123444", "0")]
    [DataRow("111122-111122", "1")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
