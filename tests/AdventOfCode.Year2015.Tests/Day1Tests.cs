namespace AdventOfCode.Year2015.Tests
{
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015.Day1;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;

    [TestClass]
    public class Day1Tests : ChallengeTests<Day1Challenge>
    {
        [TestMethod]
        [DataRow("(())", "0")]
        [DataRow("()()", "0")]
        [DataRow("(((", "3")]
        [DataRow("(()(()(", "3")]
        [DataRow("))(((((", "3")]
        [DataRow("())", "-1")]
        [DataRow("))(", "-1")]
        [DataRow(")))", "-3")]
        [DataRow(")())())", "-3")]
        public async Task SolvePart1(string input, string answer)
        {
            var solvedAnswer = await this.challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow(")", "1")]
        [DataRow("()())", "5")]
        public async Task SolvePart2(string input, string answer)
        {
            var solvedAnswer = await this.challenge.SolvePart2(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }
    }
}
