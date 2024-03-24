namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day5SolutionTests : SolverBaseTests<Day5Solution>
{
    [TestMethod]
    [DataRow("ugknbfddgicrmopn", "1")]
    [DataRow("aaa", "1")]
    [DataRow("jchzalrnumimnmhp", "0")]
    [DataRow("haegwjzuvuyypxyu", "0")]
    [DataRow("dvszwmarrgswjxmb", "0")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("qjhvhtzxzqqjkmpb", "1")]
    [DataRow("xxyxx", "1")]
    [DataRow("uurcxstgmygtbstg", "0")]
    [DataRow("ieodomkazucvgmuy", "0")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
