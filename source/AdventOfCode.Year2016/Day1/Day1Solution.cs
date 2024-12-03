namespace AdventOfCode.Year2016;

using AdventOfCode.Problem;
using System;
using System.Collections.Generic;

[Problem(2016, 1, "No Time for a Taxicab")]
public partial class Day1Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var instructions = ParseInput(input);
        var position = (x: 0, y: 0);
        var direction = 0;

        foreach (var instruction in instructions)
        {
            direction = instruction.Direction switch
            {
                'L' => (direction + 3) % 4,
                'R' => (direction + 1) % 4,
                _ => throw new Exception("Invalid direction")
            };

            position = direction switch
            {
                0 => (position.x, position.y + instruction.Distance),
                1 => (position.x + instruction.Distance, position.y),
                2 => (position.x, position.y - instruction.Distance),
                3 => (position.x - instruction.Distance, position.y),
                _ => throw new Exception("Invalid direction")
            };
        }

        return Math.Abs(position.x) + Math.Abs(position.y);
    }

    public object? PartTwo(string input)
    {
        var instructions = ParseInput(input);
        var position = (x: 0, y: 0);
        var direction = 0;

        var visited = new HashSet<(int x, int y)> { position };

        foreach (var instruction in instructions)
        {
            direction = instruction.Direction switch
            {
                'L' => (direction + 3) % 4,
                'R' => (direction + 1) % 4,
                _ => throw new Exception("Invalid direction")
            };

            for (var i = 0; i < instruction.Distance; i++)
            {
                position = direction switch
                {
                    0 => (position.x, position.y + 1),
                    1 => (position.x + 1, position.y),
                    2 => (position.x, position.y - 1),
                    3 => (position.x - 1, position.y),
                    _ => throw new Exception("Invalid direction")
                };

                if (visited.Contains(position))
                {
                    return Math.Abs(position.x) + Math.Abs(position.y);
                }
                visited.Add(position);
            }
        }

        throw new Exception("No location visited twice");
    }

    private static IEnumerable<Instruction> ParseInput(string input)
    {
        foreach (var instruction in input.Split(", "))
        {
            yield return new Instruction(instruction[0], int.Parse(instruction[1..]));
        }
    }

    private record Instruction(char Direction, int Distance);
}
