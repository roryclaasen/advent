﻿// ------------------------------------------------------------------------------
// <copyright file="Day1Tests.cs" company="PlaceholderCompany">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day1Tests : ChallengeTests<Day1Challenge>
    {
        [TestMethod]
        [DataRow("(())", "0")]
        [DataRow("()()", "0")]
        [DataRow("(((", "3")]
        [DataRow("(()(()(", "3")]
        [DataRow("))(((((", "3")]
        [DataRow("())", "-1")]
        [DataRow("))(", "-1")]
        [DataRow(")))", "-3")]
        [DataRow(")())())", "-3")]
        public async Task SolvePart1(string input, string answer)
        {
            var solvedAnswer = await this.challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow(")", "1")]
        [DataRow("()())", "5")]
        public async Task SolvePart2(string input, string answer)
        {
            var solvedAnswer = await this.challenge.SolvePart2(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }
    }
}
