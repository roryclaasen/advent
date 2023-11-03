namespace AdventOfCode.Year2022.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day6SolutionTests : SolverBaseTests<Day6Solution>
{
    [TestMethod]
    [DataRow("mjqjpqmgbljsphdztnvjfqwrcgsmlb", "7")]
    [DataRow("bvwbjplbgvbhsrlpgdmjqwftvncz", "5")]
    [DataRow("nppdvjthqldpwncqszvftbrmjlhg", "6")]
    [DataRow("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", "10")]
    [DataRow("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", "11")]
    public void SolvePart1(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow("mjqjpqmgbljsphdztnvjfqwrcgsmlb", "19")]
    [DataRow("bvwbjplbgvbhsrlpgdmjqwftvncz", "23")]
    [DataRow("nppdvjthqldpwncqszvftbrmjlhg", "23")]
    [DataRow("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", "29")]
    [DataRow("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", "26")]
    public void SolvePart2(string input, string answer)
    {
        Assert.IsNotNull(this.Solver);

        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
