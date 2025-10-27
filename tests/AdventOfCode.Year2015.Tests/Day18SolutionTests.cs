// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day18SolutionTests : SolverBaseTests<Day18Solution>
{
    [TestInitialize]
    public override void SetUp()
    {
        base.SetUp();
        this.Solver.GridSize = 6;
        this.Solver.Steps = 4;
    }

    [TestMethod]
    [DataRow(@".#.#.#
...##.
#....#
..#...
#.#..#
####..", "4")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@".#.#.#
...##.
#....#
..#...
#.#..#
####..", "17")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);
        this.Solver.Steps = 5;

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
