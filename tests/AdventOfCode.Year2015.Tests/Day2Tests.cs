namespace AdventOfCode.Year2015.Tests
{
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;
    using System.Threading.Tasks;

    [TestClass]
    public class Day2Tests : ChallengeTests<Day2Challenge>
    {
        [TestMethod]
        [DataRow("2x3x4", "58")]
        [DataRow("1x1x10", "43")]
        public async Task SolvePart1(string input, string answer)
        {
            var solvedAnswer = await this.challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow("2x3x4", "34")]
        [DataRow("1x1x10", "14")]
        public async Task SolvePart2(string input, string answer)
        {
            var solvedAnswer = await this.challenge.SolvePart2(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }
    }
}
