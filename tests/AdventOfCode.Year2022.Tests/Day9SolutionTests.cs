// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2022.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day9SolutionTests : SolverBaseTests<Day9Solution>
{
    [TestMethod]
    [DataRow(@"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2", "13")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2", "1")]
    [DataRow(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20", "36")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
