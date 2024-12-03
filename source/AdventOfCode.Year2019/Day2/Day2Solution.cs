namespace AdventOfCode.Year2019;

using AdventOfCode.Problem;
using System;
using System.Diagnostics;
using System.Linq;

[Problem(2019, 2, "1202 Program Alarm")]
public partial class Day2Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var program = input.Split(',').Select(int.Parse).ToArray();
        var position = 0;
        while (program[position] != 99)
        {
            var resultPosition = program[position + 3];
            program[resultPosition] = program[position] switch
            {
                1 => program[program[position + 1]] + program[program[position + 2]],
                2 => program[program[position + 1]] * program[program[position + 2]],
                _ => throw new InvalidOperationException($"Unknown OpCode: {program[position]}"),
            };

            position += 4;
        }

        return program[0];
    }

    public object? PartTwo(string input)
    {
        var program = input.Split(',').Select(int.Parse).ToArray();
        for (var noun = 0; noun < 100; noun++)
        {
            for (var verb = 0; verb < 100; verb++)
            {
                var memory = program.ToArray();
                memory[1] = noun;
                memory[2] = verb;

                var position = 0;
                while (memory[position] != 99)
                {
                    var resultPosition = memory[position + 3];
                    memory[resultPosition] = memory[position] switch
                    {
                        1 => memory[memory[position + 1]] + memory[memory[position + 2]],
                        2 => memory[memory[position + 1]] * memory[memory[position + 2]],
                        _ => throw new InvalidOperationException($"Unknown OpCode: {memory[position]}"),
                    };

                    position += 4;
                }

                if (memory[0] == 19690720)
                {
                    return 100 * noun + verb;
                }
            }
        }

        throw new UnreachableException("Failed to find a solution");
    }
}
