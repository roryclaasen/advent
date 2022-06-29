// ------------------------------------------------------------------------------
// <copyright file="Day4Tests.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day4Tests : ChallengeTests<Day4Challenge>
    {
        [TestMethod]
        [DataRow("abcdef", "609043")]
        [DataRow("pqrstuv", "1048970")]
        public async Task Day4SolvePart1(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }
    }
}
