// ------------------------------------------------------------------------------
// <copyright file="Day6Tests.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day6Tests : ChallengeTests<Day6Challenge>
    {
        [TestMethod]
        [DataRow("turn on 0,0 through 999,999", "1000000")]
        [DataRow("toggle 0,0 through 999,0", "1000")]
        [DataRow("turn off 499,499 through 500,500", "0")]
        public async Task Day6SolvePart1(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow("turn on 0,0 through 0,0", "1")]
        [DataRow("toggle 0,0 through 999,999", "2000000")]
        public async Task Day6SolvePart2(string input, string answer)
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

