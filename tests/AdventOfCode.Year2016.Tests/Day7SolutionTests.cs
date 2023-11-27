namespace AdventOfCode.Year2016.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day7SolutionTests : SolverBaseTests<Day7Solution>
{
    [TestMethod]
    [DataRow("abba[mnop]qrst", "1")]
    [DataRow("abcd[bddb]xyyx", "0")]
    [DataRow("aaaa[qwer]tyui", "0")]
    [DataRow("ioxxoj[asdfgh]zxcvbn", "1")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("aba[bab]xyz", "1")]
    [DataRow("xyx[xyx]xyx", "0")]
    [DataRow("aaa[kek]eke", "1")]
    [DataRow("zazbz[bzb]cdb", "1")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
