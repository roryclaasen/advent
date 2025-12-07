// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day6SolutionTests : SolverBaseTests<Day6Solution>
{
    [TestMethod]
    [DataRow(@"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ", "4277556")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ", "3263827")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
