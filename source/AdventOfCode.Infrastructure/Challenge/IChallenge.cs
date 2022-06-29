// ------------------------------------------------------------------------------
// <copyright file="IChallenge.cs" company="PlaceholderCompany">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public interface IChallenge
    {
        public Task<string> SolvePart1(string input);

        public Task<string> SolvePart2(string input);
    }
}
