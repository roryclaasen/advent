namespace AdventOfCode.Year2015.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day10SolutionTests : SolverBaseTests<Day10Solution>
{
    [TestMethod]
    [DataRow("1", "11")]
    [DataRow("11", "21")]
    [DataRow("21", "1211")]
    [DataRow("1211", "111221")]
    [DataRow("111221", "312211")]
    public void LookAndSay(string input, string answer)
    {
        var solvedAnswer = Day10Solution.LookAndSay(input);
        Assert.AreEqual(answer, solvedAnswer);
    }
}
