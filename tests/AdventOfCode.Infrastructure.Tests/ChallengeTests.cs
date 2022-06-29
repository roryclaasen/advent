// ------------------------------------------------------------------------------
// <copyright file="ChallengeTests.cs" company="PlaceholderCompany">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure.Tests
{
    public class ChallengeTests<TChallenge> where TChallenge : IChallenge, new()
    {
        protected TChallenge challenge;

        [TestInitialize]
        public void SetUp()
        {
            challenge = new TChallenge();
        }
    }
}
