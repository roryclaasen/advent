namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day20SolutionTests : SolverBaseTests<Day20Solution>
{
    [TestMethod]
    [DataRow("30", "2")]
    [DataRow("40", "3")]
    [DataRow("50", "4")]
    [DataRow("60", "4")]
    [DataRow("70", "4")]
    [DataRow("80", "6")]
    [DataRow("90", "6")]
    [DataRow("100", "6")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
