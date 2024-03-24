namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day17SolutionTests : SolverBaseTests<Day17Solution>
{
    [TestInitialize]
    public override void SetUp()
    {
        base.SetUp();
        this.Solver.MaxLiters = 25;
    }

    [TestMethod]
    [DataRow(@"20
15
10
5
5", "4")]

    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"20
15
10
5
5", "2")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
