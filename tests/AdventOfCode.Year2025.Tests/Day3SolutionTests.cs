// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day3SolutionTests : SolverBaseTests<Day3Solution>
{
    [TestMethod]
    [DataRow(@"987654321111111
811111111111119
234234234234278
818181911112111", "357")]
    [DataRow("987654321111111", "98")]
    [DataRow("811111111111119", "89")]
    [DataRow("234234234234278", "78")]
    [DataRow("818181911112111", "92")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"987654321111111
811111111111119
234234234234278
818181911112111", "3121910778619")]
    [DataRow("987654321111111", "987654321111")]
    [DataRow("811111111111119", "811111111119")]
    [DataRow("234234234234278", "434234234278")]
    [DataRow("818181911112111", "888911112111")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
