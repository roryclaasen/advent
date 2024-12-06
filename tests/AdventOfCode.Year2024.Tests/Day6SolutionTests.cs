namespace AdventOfCode.Year2024.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day6SolutionTests : SolverBaseTests<Day6Solution>
{
    [TestMethod]
    [DataRow(@"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...", "41")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...", "6")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
