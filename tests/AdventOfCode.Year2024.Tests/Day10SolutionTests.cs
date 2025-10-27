// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day10SolutionTests : SolverBaseTests<Day10Solution>
{
    [TestMethod]
    [DataRow(@"0123
1234
8765
9876", "1")]
    [DataRow(@"...0...
...1...
...2...
6543456
7.....7
8.....8
9.....9", "2")]
    [DataRow(@"10..9..
2...8..
3...7..
4567654
...8..3
...9..2
.....01", "3")]
    [DataRow(@"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732", "36")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@".....0.
..4321.
..5..2.
..6543.
..7..4.
..8765.
..9....", "3")]
    [DataRow(@"..90..9
...1.98
...2..7
6543456
765.987
876....
987....", "13")]
    [DataRow(@"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732", "81")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
