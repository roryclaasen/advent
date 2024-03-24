namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day19SolutionTests : SolverBaseTests<Day19Solution>
{
    [TestMethod]
    [DataRow(@"H => HO
H => OH
O => HH

HOH", "4")]
[DataRow(@"H => HO
H => OH
O => HH

HOHOHO", "7")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"e => H
e => O
H => HO
H => OH
O => HH

HOHOHO", "6")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
