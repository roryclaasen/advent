namespace AdventOfCode.Year2023.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day7SolutionTests : SolverBaseTests<Day7Solution>
{
    [TestMethod]
    [DataRow(@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483", "6440")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483", "5905")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
