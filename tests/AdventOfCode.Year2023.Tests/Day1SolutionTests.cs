// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2023.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day1SolutionTests : SolverBaseTests<Day1Solution>
{
    [TestMethod]
    [DataRow("1abc2", "12")]
    [DataRow("pqr3stu8vwx", "38")]
    [DataRow("a1b2c3d4e5f", "15")]
    [DataRow("treb7uchet", "77")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("two1nine", "29")]
    [DataRow("eightwothree", "83")]
    [DataRow("abcone2threexyz", "13")]
    [DataRow("xtwone3four", "24")]
    [DataRow("4nineeightseven2", "42")]
    [DataRow("zoneight234", "14")]
    [DataRow("7pqrstsixteen", "76")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
