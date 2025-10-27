// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2018.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day2SolutionTests : SolverBaseTests<Day2Solution>
{
    [TestMethod]
    [DataRow(@"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab", "12")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz", "fgij")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
