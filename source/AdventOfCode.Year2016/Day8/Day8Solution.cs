namespace AdventOfCode.Year2016;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;

[Problem(2016, 8, "Two-Factor Authentication")]
public class Day8Solution : ISolver
{
    public object? PartOne(string input)
    {
        var instructions = this.ParseInput(input);
        var screen = this.RunInstructions(instructions);

        var count = 0;
        for (var y = 0; y < screen.GetLength(0); y++)
        {
            for (var x = 0; x < screen.GetLength(1); x++)
            {
                if (screen[y, x])
                {
                    count++;
                }
            }
        }

        return count;
    }

    public object? PartTwo(string input)
    {
        var instructions = this.ParseInput(input);
        var screen = this.RunInstructions(instructions);
        return screen.Render();
    }

    bool[,] RunInstructions(IEnumerable<Instruction> instructions)
    {
        var screen = new bool[6, 50];
        foreach (var ins in instructions)
        {
            if (ins.Operation == "rect")
            {
                for (var y = 0; y < ins.B; y++)
                {
                    for (var x = 0; x < ins.A; x++)
                    {
                        screen[y, x] = true;
                    }
                }
            }
            else if (ins.Operation == "rotate row")
            {
                var row = new bool[screen.GetLength(1)];
                for (var x = 0; x < screen.GetLength(1); x++)
                {
                    row[(x + ins.B) % screen.GetLength(1)] = screen[ins.A, x];
                }
                for (var x = 0; x < screen.GetLength(1); x++)
                {
                    screen[ins.A, x] = row[x];
                }
            }
            else if (ins.Operation == "rotate column")
            {
                var column = new bool[screen.GetLength(0)];
                for (var y = 0; y < screen.GetLength(0); y++)
                {
                    column[(y + ins.B) % screen.GetLength(0)] = screen[y, ins.A];
                }
                for (var y = 0; y < screen.GetLength(0); y++)
                {
                    screen[y, ins.A] = column[y];
                }
            }
            else
            {
                throw new Exception($"Unknown instruction: {ins.Operation}");
            }
        }

        return screen;
    }

    IEnumerable<Instruction> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            if (line.StartsWith("rect"))
            {
                var parts = line.Split(' ');
                var size = parts[1].Split('x');
                yield return new Instruction("rect", int.Parse(size[0]), int.Parse(size[1]));
            }
            else if (line.StartsWith("rotate row"))
            {
                var parts = line.Split(' ');
                var row = int.Parse(parts[2].Split('=')[1]);
                var amount = int.Parse(parts[4]);
                yield return new Instruction("rotate row", row, amount);
            }
            else if (line.StartsWith("rotate column"))
            {
                var parts = line.Split(' ');
                var column = int.Parse(parts[2].Split('=')[1]);
                var amount = int.Parse(parts[4]);
                yield return new Instruction("rotate column", column, amount);
            }
            else
            {
                throw new Exception($"Unknown instruction: {line}");
            }
        }
    }

    record Instruction(string Operation, int A, int B);
}
