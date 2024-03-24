namespace AdventOfCode.Year2016.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day3SolutionTests : SolverBaseTests<Day3Solution>
{
    [TestMethod]
    [DataRow("5 10 25", "0")]
    [DataRow("3 4 5", "1")]
    [DataRow(@"5 10 25
3 4 5", "1")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"101 301 501
102 302 502
103 303 503
201 401 601
202 402 602
203 403 603", "6")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
