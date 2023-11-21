namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2015, 23, "Opening the Turing Lock")]
public partial class Day23Solution : ISolver
{
    public object? PartOne(string input)
    {
        var instructions = ParseInput(input);
        var result = RunInstructions(instructions);
        return result["b"];
    }

    public object? PartTwo(string input)
    {
        var instructions = ParseInput(input);
        var result = RunInstructions(instructions, new Dictionary<string, uint> { { "a", 1 }, { "b", 0 } });
        return result["b"];
    }

    Dictionary<string, uint> RunInstructions(IEnumerable<Instruction> instructions, Dictionary<string, uint>? sartingRegister = null)
    {
        var register = sartingRegister ?? new Dictionary<string, uint> { { "a", 0 }, { "b", 0 } };

        var pointer = 0;
        while (pointer >= 0 && pointer < instructions.Count())
        {
            var instruction = instructions.ElementAt(pointer);
            switch (instruction.Operation)
            {
                case "hlf":
                    register[instruction.Register] /= 2;
                    pointer++;
                    break;
                case "tpl":
                    register[instruction.Register] *= 3;
                    pointer++;
                    break;
                case "inc":
                    register[instruction.Register]++;
                    pointer++;
                    break;
                case "jmp":
                    pointer += instruction.Offset;
                    break;
                case "jie":
                    if (register[instruction.Register] % 2 == 0)
                    {
                        pointer += instruction.Offset;
                    }
                    else
                    {
                        pointer++;
                    }
                    break;
                case "jio":
                    if (register[instruction.Register] == 1)
                    {
                        pointer += instruction.Offset;
                    }
                    else
                    {
                        pointer++;
                    }
                    break;
                default:
                    throw new Exception($"Unknown instruction: {instruction.Operation}");
            }
        }

        return register;
    }

    IEnumerable<Instruction> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {

            var match = InstructionRegex().Match(line);
            if (!match.Success)
            {
                throw new System.Exception($"Failed to parse instruction: {line}");
            }
            var operation = match.Groups["operation"].Value;
            var register = match.Groups["register"].Value;
            var offset = int.TryParse(match.Groups["offset"].Value, out var intOffset) ? intOffset : 0;

            yield return new Instruction(operation, register, offset);
        }
    }

    record Instruction(string Operation, string Register, int Offset);

    [GeneratedRegex("^(?<operation>\\w+) (?<register>[ab])?(, )?(?<offset>[+-]\\d+)?$")]
    private static partial Regex InstructionRegex();
}
