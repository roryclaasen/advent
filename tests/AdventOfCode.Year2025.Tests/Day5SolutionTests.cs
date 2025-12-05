// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day5SolutionTests : SolverBaseTests<Day5Solution>
{
    [TestMethod]
    [DataRow(@"3-5
10-14
16-20
12-18

1
5
8
11
17
32", "3")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"3-5
10-14
16-20
12-18

1
5
8
11
17
32", "14")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
