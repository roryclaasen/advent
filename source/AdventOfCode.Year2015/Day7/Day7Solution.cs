namespace AdventOfCode.Year2015;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2015, 7, "Some Assembly Required")]
public partial class Day7Solution : IProblemSolver
{
    public object? PartOne(string input) => this.RunInput(input)["a"];

    public object? PartTwo(string input)
    {
        var instructions = this.ParseInput(input);
        var part1Result = RunInstructions(new List<InstructionBase>(instructions));
        var aValue = part1Result["a"];

        var partTwoIns = new List<InstructionBase>(instructions);
        partTwoIns.RemoveAll(i => i.Output == "b");
        partTwoIns.Add(new SetValueInstruction("b", aValue));

        var part2Result = RunInstructions(partTwoIns);
        return part2Result["a"];
    }

    public Dictionary<string, ushort> RunInput(string input)
    {
        var instructions = this.ParseInput(input);
        return RunInstructions(instructions.ToList());
    }

    private static Dictionary<string, ushort> RunInstructions(List<InstructionBase> instructions)
    {
        var wires = new Dictionary<string, ushort>();

        bool TryGetValueOrWire([NotNullWhen(true)] string? input, out ushort value)
        {
            if (input is not null)
            {
                if (ushort.TryParse(input, out value))
                {
                    return true;
                }

                if (wires?.TryGetValue(input, out value) ?? false)
                {
                    return true;
                }
            }

            value = 0;
            return false;
        }

        while (instructions.Count != 0)
        {
            foreach (var instruction in instructions)
            {
                if (instruction is SetValueInstruction setValue)
                {
                    wires[setValue.Output] = setValue.Value;
                }
                else if (instruction is SetValueFromWireInstruction setValueFromWire)
                {
                    if (!wires.TryGetValue(setValueFromWire.A, out var value))
                    {
                        continue;
                    }

                    wires[setValueFromWire.Output] = value;
                }
                else if (instruction is BitWiseInstruction bitWise)
                {
                    if (!TryGetValueOrWire(bitWise.A, out var a))
                    {
                        continue;
                    }

                    if (!TryGetValueOrWire(bitWise.B, out var b) && !string.IsNullOrWhiteSpace(bitWise.B))
                    {
                        continue;
                    }

                    wires[bitWise.Output] = bitWise.Operation switch
                    {
                        Operation.And => (ushort)(a & b),
                        Operation.Or => (ushort)(a | b),
                        Operation.Not => (ushort)(~a & ushort.MaxValue),
                        Operation.LShift => (ushort)(a << b),
                        Operation.RShift => (ushort)(a >> b),
                        _ => throw new UnreachableException()
                    };
                }
                else
                {
                    throw new NotImplementedException("Unknown instruction type");
                }

                instructions.Remove(instruction);
                break;
            }
        }

        return wires;
    }

    private IEnumerable<InstructionBase> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            if (this.SetValueRegex().Match(line) is Match setValueMatch && setValueMatch.Success)
            {
                var output = setValueMatch.Groups["Output"].Value;
                if (ushort.TryParse(setValueMatch.Groups["Value"].Value, out var value))
                {
                    yield return new SetValueInstruction(output, value);
                }
                else if (setValueMatch.Groups["A"].Value is string a)
                {
                    yield return new SetValueFromWireInstruction(output, a);
                }
                else
                {
                    throw new UnreachableException();
                }
            }
            else if (this.BitWiseRegex().Match(line) is Match bitWiseMatch && bitWiseMatch.Success)
            {
                var a = bitWiseMatch.Groups["A"].Value;
                var b = bitWiseMatch.Groups["B"].Value;
                var operation = bitWiseMatch.Groups["Operation"].Value switch
                {
                    "AND" => Operation.And,
                    "OR" => Operation.Or,
                    "NOT" => Operation.Not,
                    "LSHIFT" => Operation.LShift,
                    "RSHIFT" => Operation.RShift,
                    _ => throw new Exception("Unknown operation")
                };
                var output = bitWiseMatch.Groups["Output"].Value;
                yield return new BitWiseInstruction(output, operation, a, b);
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    public abstract record InstructionBase(string Output);

    private record SetValueInstruction(string Output, ushort Value) : InstructionBase(Output);

    private record SetValueFromWireInstruction(string Output, string A) : InstructionBase(Output);

    private enum Operation
    {
        And,
        Or,
        Not,
        LShift,
        RShift
    }

    private record BitWiseInstruction(string Output, Operation Operation, string A, string? B = null) : InstructionBase(Output);

    [GeneratedRegex("^(?:(?<Value>\\d+)|(?<A>\\w+)) -> (?<Output>\\w+)$", RegexOptions.Singleline)]
    private partial Regex SetValueRegex();

    [GeneratedRegex("^(?:(?<Operation>NOT) (?<A>\\w+)|(?<A>\\w+) (?<Operation>AND|OR|LSHIFT|RSHIFT) (?<B>\\w+)) -> (?<Output>\\w+)$", RegexOptions.Singleline)]
    private partial Regex BitWiseRegex();
}
