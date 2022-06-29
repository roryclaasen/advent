namespace AdventOfCode.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    public abstract class Runner<TChallenge> : IRunner
        where TChallenge : IChallenge, new()
    {
        private readonly Lazy<TChallenge> challenge = new Lazy<TChallenge>(() => new());

        public abstract int Year { get; }

        public abstract int Day { get; }

        public abstract string Input { get; }

        public Task<string> SolvePart1() => this.challenge.Value.SolvePart1(this.Input);

        public Task<string> SolvePart2() => this.challenge.Value.SolvePart2(this.Input);
    }
}
