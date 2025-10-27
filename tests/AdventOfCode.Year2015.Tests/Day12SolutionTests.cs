// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day12SolutionTests : SolverBaseTests<Day12Solution>
{
    [TestMethod]
    [DataRow("[1,2,3]", "6")]
    [DataRow("{\"a\":2,\"b\":4}", "6")]
    [DataRow("[[[3]]]", "3")]
    [DataRow("{\"a\":{\"b\":4},\"c\":-1}", "3")]
    [DataRow("{\"a\":[-1,1]}", "0")]
    [DataRow("[-1,{\"a\":1}]", "0")]
    [DataRow("[]", "0")]
    [DataRow("{}", "0")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("[1,2,3]", "6")]
    [DataRow("[1,{\"c\":\"red\",\"b\":2},3]", "4")]
    [DataRow("{\"d\":\"red\",\"e\":[1,2,3,4],\"f\":5}", "0")]
    [DataRow("[1,\"red\",5]", "6")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
