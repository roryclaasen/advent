namespace AdventOfCode.Year2023.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day9SolutionTests : SolverBaseTests<Day9Solution>
{
    [TestMethod]
    [DataRow("0 3 6 9 12 15", "18")]
    [DataRow("1 3 6 10 15 21", "28")]
    [DataRow("10 13 16 21 30 45", "68")]
    [DataRow(@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45", "114")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("10 13 16 21 30 45", "5")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
