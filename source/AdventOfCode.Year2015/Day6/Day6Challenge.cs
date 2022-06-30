// ------------------------------------------------------------------------------
// <copyright file="Day6Challenge.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure;

    public class Day6Challenge : IChallenge
    {
        public Task<string> SolvePart1(string input)
        {
            var grid = this.MakeGrid();
            foreach (var line in input.Split(Environment.NewLine))
            {
                var instructions = line.Split(' ');
                var isToggle = instructions[0] == "toggle";
                var action = isToggle ? "toggle" : instructions[1];
                var start = StringHelper.ToPoint(instructions[isToggle ? 1 : 2]);
                var end = StringHelper.ToPoint(instructions[isToggle ? 3 : 4]);

                for (var x = start.X; x <= end.X; x++)
                {
                    for (var y = start.Y; y <= end.Y; y++)
                    {
                        if (isToggle)
                        {
                            grid[x, y] = grid[x, y] == 1 ? 0 : 1;
                        }
                        else if (action == "on")
                        {
                            grid[x, y] = 1;
                        }
                        else
                        {
                            grid[x, y] = 0;
                        }
                    }
                }
            }

            var lightsOn = 0;
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    if (grid[x, y] == 1)
                    {
                        lightsOn++;
                    }
                }
            }

            return this.Answer(lightsOn);
        }

        public Task<string> SolvePart2(string input)
        {
            var grid = this.MakeGrid();
            foreach (var line in input.Split(Environment.NewLine))
            {
                var instructions = line.Split(' ');
                var isToggle = instructions[0] == "toggle";
                var action = isToggle ? "toggle" : instructions[1];
                var start = StringHelper.ToPoint(instructions[isToggle ? 1 : 2]);
                var end = StringHelper.ToPoint(instructions[isToggle ? 3 : 4]);

                for (var x = start.X; x <= end.X; x++)
                {
                    for (var y = start.Y; y <= end.Y; y++)
                    {
                        if (isToggle)
                        {
                            grid[x, y] += 2;
                        }
                        else if (action == "on")
                        {
                            grid[x, y]++;
                        }
                        else if (grid[x, y] > 0)
                        {
                            grid[x, y]--;
                        }
                    }
                }
            }

            var brightness = 0;
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    brightness += grid[x, y];
                }
            }

            return this.Answer(brightness);
        }

        private int[,] MakeGrid() => new int[1000, 1000];
    }
}
