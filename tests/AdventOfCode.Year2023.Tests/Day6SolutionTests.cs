namespace AdventOfCode.Year2023.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day6SolutionTests : SolverBaseTests<Day6Solution>
{
    [TestMethod]
    [DataRow(@"Time:      7  15   30
Distance:  9  40  200", "288")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"Time:      7  15   30
Distance:  9  40  200", "71503")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
