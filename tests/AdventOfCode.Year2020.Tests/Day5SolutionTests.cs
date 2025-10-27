// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2020.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day5SolutionTests : SolverBaseTests<Day5Solution>
{
    [TestMethod]
    [DataRow("BFFFBBFRRR", "567")]
    [DataRow("FFFBBBFRRR", "119")]
    [DataRow("BBFFBBFRLL", "820")]
    [DataRow(@"BFFFBBFRRR
FFFBBBFRRR
BBFFBBFRLL", "820")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
