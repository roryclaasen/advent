// ------------------------------------------------------------------------------
// <copyright file="Day3Tests.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day3Tests : ChallengeTests<Day3Challenge>
    {
        [TestMethod]
        [DataRow(">", "2")]
        [DataRow("^>v<", "4")]
        [DataRow("^v^v^v^v^v", "2")]
        public async Task Day3SolvePart1(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow("^v", "3")]
        [DataRow("^>v<", "3")]
        [DataRow("^v^v^v^v^v", "11")]
        public async Task Day3SolvePart2(string input, string answer)
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
