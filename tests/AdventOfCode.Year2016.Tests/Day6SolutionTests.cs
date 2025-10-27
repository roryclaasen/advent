// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016.Tests;

using AdventOfCode.Shared.Tests;
using System.Diagnostics.CodeAnalysis;

[TestClass]
public class Day6SolutionTests : SolverBaseTests<Day6Solution>
{
    [TestInitialize]
    [MemberNotNull(nameof(this.Solver))]
    public override void SetUp()
    {
        base.SetUp();

        this.Solver.CharacterCount = 6;
    }

    [TestMethod]
    [DataRow(@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar", "easter")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar", "advent")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
