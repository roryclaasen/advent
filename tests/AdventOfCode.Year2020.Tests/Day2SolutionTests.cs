// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2020.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day2SolutionTests : SolverBaseTests<Day2Solution>
{
    [TestMethod]
    [DataRow(@"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc", "2")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc", "1")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
