// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2023;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2023, 2, "Cube Conundrum")]
public partial class Day2Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var games = ParseInput(input)
            .Where(g => !g.Reveals.Any(r => r.Red > 12))
            .Where(g => !g.Reveals.Any(r => r.Blue > 14))
            .Where(g => !g.Reveals.Any(r => r.Green > 13));
        return games.Sum(g => g.Id);
    }

    public object? PartTwo(string input)
    {
        static int MinCubes(Game game, Func<GameReveal, int> selector)
            => Math.Max(1, game.Reveals.Select(selector).Max());

        var games = ParseInput(input)
            .Select(g => MinCubes(g, r => r.Red) * MinCubes(g, r => r.Blue) * MinCubes(g, r => r.Green));

        return games.Sum();
    }

    private static IEnumerable<Game> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var part = line.Split(':');
            var id = int.Parse(part[0].Replace("Game ", string.Empty));
            var bags = part[1].Split(';').Select(GameReveal.ParseInput);
            yield return new Game(id, bags);
        }
    }

    private partial record class GameReveal(int Red, int Blue, int Green)
    {
        [GeneratedRegex("(?<amount>\\d+) (?<color>red|blue|green)")]
        private static partial Regex ColorRegex();

        public static GameReveal ParseInput(string input)
        {
            var red = 0;
            var blue = 0;
            var green = 0;

            foreach (var match in input.Split(',').Select(s => ColorRegex().Match(s)))
            {
                if (!match.Success)
                {
                    throw new Exception("Unable to parse color");
                }

                var color = match.Groups["color"].Value;
                var value = int.Parse(match.Groups["amount"].Value);
                if (color == "red")
                {
                    red += value;
                }
                else if (color == "blue")
                {
                    blue += value;
                }
                else if (color == "green")
                {
                    green += value;
                }
            }

            return new GameReveal(red, blue, green);
        }
    }

    private record Game(int Id, IEnumerable<GameReveal> Reveals);
}
