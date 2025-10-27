// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2017.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day2SolutionTests : SolverBaseTests<Day2Solution>
{
    [TestMethod]
    [DataRow(@"5 1 9 5
7 5 3
2 4 6 8", "18")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"5 9 2 8
9 4 7 3
3 8 6 5", "9")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
