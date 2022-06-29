// ------------------------------------------------------------------------------
// <copyright file="Day2Challenge.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure;

    public class Day2Challenge : IChallenge
    {
        public Task<string> SolvePart1(string input)
        {
            var parsed = this.ParseInput(input);

            var total = 0;

            foreach (var box in parsed)
            {
                var sides = new List<int>
                {
                    box.Length * box.Width,
                    box.Width * box.Height,
                    box.Height * box.Length,
                };

                total += (sides.Sum() * 2) + sides.Min();
            }

            return this.Answer(total);
        }

        public Task<string> SolvePart2(string input)
        {
            var parsed = this.ParseInput(input);

            var total = 0;

            foreach (var box in parsed)
            {
                var measurements = new List<int>
                {
                    box.Length,
                    box.Width,
                    box.Height,
                };
                measurements.Sort();

                total += (measurements.Take(2).Sum() * 2) + (box.Length * box.Width * box.Height);
            }

            return this.Answer(total);
        }

        private IEnumerable<Dimension> ParseInput(string input) => input
            .Split(Environment.NewLine)
            .Select(line => line.Split('x').Select(int.Parse))
            .Select(line =>
            {
                var array = line.ToArray();
                return new Dimension(array[0], array[1], array[2]);
            });

        private struct Dimension
        {
            public readonly int Length;
            public readonly int Width;
            public readonly int Height;

            public Dimension(int length, int width, int height)
            {
                this.Length = length;
                this.Width = width;
                this.Height = height;
            }
        }
    }
}
