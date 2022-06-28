namespace AdventOfCode.Year2015
{
    using AdventOfCode.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Day2Challenge : IChallenge
    {
        private struct Dimenstion
        {
            public readonly int length;
            public readonly int width;
            public readonly int height;

            public Dimenstion(int length, int width, int height)
            {
                this.length = length;
                this.width = width;
                this.height = height;
            }
        }

        private IEnumerable<Dimenstion> ParseInput(string input) => input
            .Split(Environment.NewLine)
            .Select(line => line.Split('x').Select(int.Parse))
            .Select(line =>
            {
                var array = line.ToArray();
                return new Dimenstion(array[0], array[1], array[2]);
            });


        public Task<string> SolvePart1(string input)
        {
            var parsed = this.ParseInput(input);

            var total = 0;

            foreach (var box in parsed)
            {
                var sides = new List<int>
                {
                    box.length * box.width,
                    box.width * box.height,
                    box.height * box.length
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
                var sides = new List<int>
                {
                    box.length,
                    box.width,
                    box.height
                };
                sides.Sort();

                total += (sides.Take(2).Sum() * 2) + (box.length * box.width * box.height);
            }

            return this.Answer(total);
        }
    }
}