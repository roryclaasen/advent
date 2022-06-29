// ------------------------------------------------------------------------------
// <copyright file="Day5Tests.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day5Tests : ChallengeTests<Day5Challenge>
    {
        [TestMethod]
        [DataRow("ugknbfddgicrmopn", "1")]
        [DataRow("aaa", "1")]
        [DataRow("jchzalrnumimnmhp", "0")]
        [DataRow("haegwjzuvuyypxyu", "0")]
        [DataRow("dvszwmarrgswjxmb", "0")]
        public async Task Day5SolvePart1(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow("qjhvhtzxzqqjkmpb", "1")]
        [DataRow("xxyxx", "1")]
        [DataRow("uurcxstgmygtbstg", "0")]
        [DataRow("ieodomkazucvgmuy", "0")]
        public async Task Day5SolvePart2(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart2(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }
    }
}
