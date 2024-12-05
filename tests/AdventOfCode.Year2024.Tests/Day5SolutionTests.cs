namespace AdventOfCode.Year2024.Tests;

using AdventOfCode.Shared.Tests;

[TestClass]
public class Day5SolutionTests : SolverBaseTests<Day5Solution>
{
    [TestMethod]
    [DataRow(@"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47", "143")]
    public void SolvePart1(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartOne(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }

    [TestMethod]
    [DataRow(@"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47", "123")]
    public void SolvePart2(string input, string answer)
    {
        var solvedAnswer = this.Solver.PartTwo(input)?.ToString();
        Assert.AreEqual(answer, solvedAnswer);
    }
}
