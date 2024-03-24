namespace AdventOfCode.Year2016.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day9SolutionTests : SolverBaseTests<Day9Solution>
{
    [TestMethod]
    [DataRow("ADVENT", "6")]
    [DataRow("A(1x5)BC", "7")]
    [DataRow("(3x3)XYZ", "9")]
    [DataRow("A(2x2)BCD(2x2)EFG", "11")]
    [DataRow("(6x1)(1x3)A", "6")]
    [DataRow("X(8x2)(3x3)ABCY", "18")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("(3x3)XYZ", "9")]
    [DataRow("X(8x2)(3x3)ABCY", "20")]
    [DataRow("(27x12)(20x12)(13x14)(7x10)(1x12)A", "241920")]
    [DataRow("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", "445")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
