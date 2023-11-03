namespace AdventOfCode.Year2022.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day1SolutionTests : SolverBaseTests<Day1Solution>
{
    [TestMethod]
    [DataRow("", "")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("", "")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
