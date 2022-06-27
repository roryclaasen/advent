namespace AdventOfCode.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    public abstract class Runner<TChallenge> : IRunner
        where TChallenge : Challenge, new()
    {
        protected Lazy<TChallenge> Challenge = new Lazy<TChallenge>(() => new());

        public abstract int Year { get; }

        public abstract int Day { get; }

        public abstract Task<string> SolvePart1();

        public abstract Task<string> SolvePart2();
    }
}
