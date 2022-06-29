// ------------------------------------------------------------------------------
// <copyright file="Day3Challenge.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure;

    public class Day3Challenge : IChallenge
    {
        public Task<string> SolvePart1(string input)
        {
            var santa = new Point(0, 0);

            var delivered = new Dictionary<string, int>
            {
                { santa.ToString(), 1 },
            };

            foreach (var dir in input)
            {
                santa = this.MovePoint(santa, dir);

                if (!delivered.ContainsKey(santa.ToString()))
                {
                    delivered[santa.ToString()] = 1;
                }
                else
                {
                    delivered[santa.ToString()]++;
                }
            }

            return this.Answer(delivered.Count);
        }

        public Task<string> SolvePart2(string input)
        {
            var santa = new Point(0, 0);
            var robot = new Point(0, 0);

            var delivered = new Dictionary<string, int>
            {
                { santa.ToString(), 2 },
            };

            var isSanta = true;

            foreach (var dir in input)
            {
                var point = this.MovePoint(isSanta ? santa : robot, dir);

                if (!delivered.ContainsKey(point.ToString()))
                {
                    delivered[point.ToString()] = 1;
                }
                else
                {
                    delivered[point.ToString()]++;
                }

                if (isSanta)
                {
                    santa = point;
                }
                else
                {
                    robot = point;
                }

                isSanta = !isSanta;
            }

            return this.Answer(delivered.Count);
        }

        private Point MovePoint(Point point, char dir)
        {
            if (dir == '^')
            {
                point.Y++;
            }
            else if (dir == 'v')
            {
                point.Y--;
            }
            else if (dir == '>')
            {
                point.X++;
            }
            else if (dir == '<')
            {
                point.X--;
            }

            return point;
        }
    }
}
