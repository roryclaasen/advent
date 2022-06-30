// ------------------------------------------------------------------------------
// <copyright file="Runner{TChallenge}.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    public abstract class Runner<TChallenge> : IRunner
        where TChallenge : IChallenge, new()
    {
        private readonly Lazy<TChallenge> challenge = new Lazy<TChallenge>(() => new());

        /// <inheritdoc/>
        public abstract int Year { get; }

        /// <inheritdoc/>
        public abstract int Day { get; }

        /// <summary>
        /// Gets the input to pass to the challenge.
        /// </summary>
        public abstract string Input { get; }

        public Task<string> SolvePart1() => this.challenge.Value.SolvePart1(this.Input);

        public Task<string> SolvePart2() => this.challenge.Value.SolvePart2(this.Input);
    }
}
