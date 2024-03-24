namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day9SolutionTests : SolverBaseTests<Day9Solution>
{
    [TestMethod]
    [DataRow(@"London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141", "605")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141", "982")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
