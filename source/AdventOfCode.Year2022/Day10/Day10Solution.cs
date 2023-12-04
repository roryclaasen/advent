namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2022, 10, "Cathode-Ray Tube")]
public class Day10Solution : ISolver
{
    public object? PartOne(string input)
    {
        var cycles = new int[] { 20, 60, 100, 140, 180, 220 };
        return RunProgram(ParseInput(input))
            .Where(signal => cycles.Contains(signal.Cycle))
            .Sum(signal => signal.Cycle * signal.X);
    }

    public object? PartTwo(string input)
        => string.Join(Environment.NewLine,
            RunProgram(ParseInput(input))
                .Select(signal =>
                {
                    var spriteMiddle = signal.X;
                    var screenColumn = (signal.Cycle - 1) % 40;
                    return Math.Abs(spriteMiddle - screenColumn) < 2 ? '#' : ' ';
                })
                .Chunk(40)
                .Select(line => new string(line).TrimEnd())
            );

    static IEnumerable<(int Cycle, int X)> RunProgram(IEnumerable<Instruction> instructions)
    {
        var (cycle, x) = (1, 1);
        foreach (var ins in instructions)
        {
            switch (ins.Operation)
            {
                case "noop":
                    yield return (cycle++, x);
                    break;
                case "addx":
                    yield return (cycle++, x);
                    yield return (cycle++, x);
                    x += ins.Argument;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown operation: {ins.Operation}");
            }
        }
    }

    IEnumerable<Instruction> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var parts = line.Split(' ');
            yield return new Instruction(parts[0], parts.Length == 2 ? int.Parse(parts[1]) : 0);
        }
    }

    public record Instruction(string Operation, int Argument);
}
