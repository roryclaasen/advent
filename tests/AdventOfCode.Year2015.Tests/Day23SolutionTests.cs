// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day23SolutionTests : SolverBaseTests<Day23Solution>
{
    [TestMethod]
    [DataRow(@"inc b
jio b, +2
tpl b
inc b", "2")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
