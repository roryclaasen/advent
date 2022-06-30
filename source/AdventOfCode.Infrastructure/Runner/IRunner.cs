// ------------------------------------------------------------------------------
// <copyright file="IRunner.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public interface IRunner
    {
        /// <summary>
        /// Gets the year of the challenge that will be run.
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Gets the day of the chalenge that will be run.
        /// </summary>
        int Day { get; }

        Task<string> SolvePart1();

        Task<string> SolvePart2();
    }
}
