// ------------------------------------------------------------------------------
// <copyright file="Day2Tests.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day2Tests : ChallengeTests<Day3Challenge>
    {
        [TestMethod]
        [DataRow("2x3x4", "58")]
        [DataRow("1x1x10", "43")]
        public async Task Day2SolvePart1(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow("2x3x4", "34")]
        [DataRow("1x1x10", "14")]
        public async Task Day2SolvePart2(string input, string answer)
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
