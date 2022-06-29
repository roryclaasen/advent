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
        int Year { get; }

        int Day { get; }

        Task<string> SolvePart1();

        Task<string> SolvePart2();
    }
}
