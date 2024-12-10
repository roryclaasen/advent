namespace AdventOfCode.Year2024.Tests;

using AdventOfCode.Shared;
using AdventOfCode.Shared.Tests;
using CommunityToolkit.HighPerformance;

[TestClass]
public class Day9SolutionTests : SolverBaseTests<Day9Solution>
{
    [TestMethod]
    [DataRow("2333133121414131402", "1928")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("2333133121414131402", "2858")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
