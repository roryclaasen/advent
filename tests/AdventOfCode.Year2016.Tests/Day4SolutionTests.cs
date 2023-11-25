namespace AdventOfCode.Year2016.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day4SolutionTests : SolverBaseTests<Day4Solution>
{
    [TestMethod]
    [DataRow("aaaaa-bbb-z-y-x-123[abxyz]", "123")]
    [DataRow("a-b-c-d-e-f-g-h-987[abcde]", "987")]
    [DataRow("not-a-real-room-404[oarel]", "404")]
    [DataRow("totally-real-room-200[decoy]", "0")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
