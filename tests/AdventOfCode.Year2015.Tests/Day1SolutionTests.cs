namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day1SolutionTests : SolverBaseTests<Day1Solution>
{
    [TestMethod]
    [DataRow("(())", "0")]
    [DataRow("()()", "0")]
    [DataRow("(((", "3")]
    [DataRow("(()(()(", "3")]
    [DataRow("))(((((", "3")]
    [DataRow("())", "-1")]
    [DataRow("))(", "-1")]
    [DataRow(")))", "-3")]
    [DataRow(")())())", "-3")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(")", "1")]
    [DataRow("()())", "5")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
