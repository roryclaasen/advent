namespace AdventOfCode.Year2019.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day2SolutionTests : SolverBaseTests<Day2Solution>
{
    [TestMethod]
    [DataRow("1,0,0,0,99", "2")]
    [DataRow("2,3,0,3,99", "2")]
    [DataRow("2,4,4,5,99,0", "2")]
    [DataRow("1,1,1,4,99,5,6,0,99", "30")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
