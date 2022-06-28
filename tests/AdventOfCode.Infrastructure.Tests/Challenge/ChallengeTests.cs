namespace AdventOfCode.Infrastructure.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
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
