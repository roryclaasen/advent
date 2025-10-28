// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2024, 8, "Resonant Collinearity")]
public partial class Day8Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var (antentas, width, height) = ParseInput(input);

        var antinodes = new HashSet<Vector2>();
        void AddAntinode(Vector2 a, Vector2 b)
        {
            var c = GetAntinode(a, b);
            if (IsInsideBounds(c, width, height))
            {
                antinodes.Add(c);
            }
        }

        foreach (var (a, b) in antentas.Select(a => a.Combinations(2)).SelectMany(s => s))
        {
            AddAntinode(a, b);
            AddAntinode(b, a);
        }

        return antinodes.Count;
    }

    public object? PartTwo(string input)
    {
        var (antennas, width, height) = ParseInput(input);

        var antinodes = new HashSet<Vector2>();
        void AddAntinodes(Vector2 a, Vector2 b)
        {
            do
            {
                antinodes.Add(b);
                var c = GetAntinode(a, b);
                b = a;
                a = c;
            } while (IsInsideBounds(b, width, height));
        }

        foreach (var (a, b) in antennas.Select(a => a.Combinations(2)).SelectMany(s => s))
        {
            AddAntinodes(a, b);
            AddAntinodes(b, a);
        }

        return antinodes.Count;
    }

    private static bool IsInsideBounds(Vector2 node, int width, int height) => node.X >= 0 && node.X < width && node.Y >= 0 && node.Y < height;

    private static Vector2 GetAntinode(Vector2 a, Vector2 b) => (2 * a) - b;

    private static InputData ParseInput(string input)
    {
        static IEnumerable<(char Frequency, Vector2 Position)> GetAntennas(IEnumerable<string> lines)
        {
            foreach (var (line, y) in lines.WithIndex())
            {
                foreach (var (c, x) in line.WithIndex())
                {
                    if (c == '.')
                    {
                        continue;
                    }

                    yield return (c, new Vector2(x, y));
                }
            }
        }

        var lines = input.Lines();
        return new InputData(GetAntennas(lines).ToLookup(a => a.Frequency, a => a.Position), lines.Length, lines.First().Length);
    }

    private record struct InputData(ILookup<char, Vector2> Antennas, int Width, int Height);
}
