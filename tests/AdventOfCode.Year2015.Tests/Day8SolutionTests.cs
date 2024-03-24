namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day8SolutionTests : SolverBaseTests<Day8Solution>
{
    [TestMethod]
    [DataRow("\"\"", "2")]
    [DataRow("\"abc\"", "2")]
    [DataRow("\"aaa\\\"aaa\"", "3")]
    [DataRow("\"\\x27\"", "5")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("\"\"", "4")]
    [DataRow("\"abc\"", "4")]
    [DataRow("\"aaa\\\"aaa\"", "6")]
    [DataRow("\"\\x27\"", "5")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
