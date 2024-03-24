namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day6SolutionTests : SolverBaseTests<Day6Solution>
{
    [TestMethod]
    [DataRow("turn on 0,0 through 999,999", "1000000")]
    [DataRow("toggle 0,0 through 999,0", "1000")]
    [DataRow("turn off 499,499 through 500,500", "0")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("turn on 0,0 through 0,0", "1")]
    [DataRow("toggle 0,0 through 999,999", "2000000")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
