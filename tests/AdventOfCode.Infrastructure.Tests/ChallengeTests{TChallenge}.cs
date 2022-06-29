// ------------------------------------------------------------------------------
// <copyright file="ChallengeTests{TChallenge}.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure.Tests
{
    public class ChallengeTests<TChallenge>
        where TChallenge : IChallenge, new()
    {
        protected TChallenge? Challenge { get; private set; }

        [TestInitialize]
        public void SetUp()
        {
            this.Challenge = new TChallenge();
        }

        [TestMethod]
        public void CanCreateChallenge()
        {
            Assert.IsNotNull(this.Challenge);
        }
    }
}
