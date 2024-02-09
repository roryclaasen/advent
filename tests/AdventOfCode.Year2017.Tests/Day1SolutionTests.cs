namespace AdventOfCode.Year2017.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day1SolutionTests : SolverBaseTests<Day1Solution>
{
    [TestMethod]
    [DataRow("1122", "3")]
    [DataRow("1111", "4")]
    [DataRow("1234", "0")]
    [DataRow("91212129", "9")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("1212", "6")]
    [DataRow("1221", "0")]
    [DataRow("123425", "4")]
    [DataRow("123123", "12")]
    [DataRow("12131415", "4")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
