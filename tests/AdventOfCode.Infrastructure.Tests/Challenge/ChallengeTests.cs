namespace AdventOfCode.Infrastructure.Tests
{
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
