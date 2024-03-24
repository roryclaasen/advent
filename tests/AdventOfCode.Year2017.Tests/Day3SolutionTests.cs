namespace AdventOfCode.Year2017.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day3SolutionTests : SolverBaseTests<Day3Solution>
{
    [TestMethod]
    [DataRow("1", "0")]
    [DataRow("12", "3")]
    [DataRow("23", "2")]
    [DataRow("1024", "31")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    //[TestMethod]
    //[DataRow("", "")]
    //public void SolvePart2(string input, string answer)
    //{
    //    Assert.IsNotNull(this.Solver);

    //    var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
    //    Assert.AreEqual(answer, solvedAnswer);
    //}
}
