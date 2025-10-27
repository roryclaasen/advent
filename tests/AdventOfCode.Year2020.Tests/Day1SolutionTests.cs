// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2020.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day1SolutionTests : SolverBaseTests<Day1Solution>
{
    [TestMethod]
    [DataRow(@"1721
979
366
299
675
1456", "514579")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"1721
979
366
299
675
1456", "241861950")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
