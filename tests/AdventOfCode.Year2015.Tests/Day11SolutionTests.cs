namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day11SolutionTests : SolverBaseTests<Day11Solution>
{
    [TestMethod]
    [DataRow("abcdefgh", "abcdffaa")]
    [DataRow("ghijklmn", "ghjaabcc")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
