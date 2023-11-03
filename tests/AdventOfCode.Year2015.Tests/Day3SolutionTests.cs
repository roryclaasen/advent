namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day3SolutionTests : SolverBaseTests<Day3Solution>
{
    [TestMethod]
    [DataRow(">", "2")]
    [DataRow("^>v<", "4")]
    [DataRow("^v^v^v^v^v", "2")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("^v", "3")]
    [DataRow("^>v<", "3")]
    [DataRow("^v^v^v^v^v", "11")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
