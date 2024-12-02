namespace AdventOfCode.Year2019;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[Problem(2019, 3, "Crossed Wires")]
public partial class Day3Solution : ISolver
{
    public object? PartOne(string input)
    {
        var wires = ParseInput(input);

        var wire1Path = GetPath(wires[0]);
        var wire2Path = GetPath(wires[1]);
        return wire1Path
            .Intersect(wire2Path, new Vector3ComparerAsVector2())
            .Select(s => Math.Abs(s.X) + Math.Abs(s.Y))
            .Min();
    }

    public object? PartTwo(string input)
    {
        var wires = ParseInput(input);

        var wire1Path = GetPath(wires[0]);
        var wire2Path = GetPath(wires[1]);
        return wire1Path
            .Intersect(wire2Path, new Vector3ComparerAsVector2())
            .Select(i => wire1Path.First(s => s.X == i.X && s.Y == i.Y).Z + wire2Path.First(s => s.X == i.X && s.Y == i.Y).Z)
            .Min();
    }

    private static IEnumerable<Vector3> GetPath(IEnumerable<Instruction> instructions)
    {
        var current = Vector3.Zero;

        foreach (var instruction in instructions)
        {
            for (var i = 0; i < instruction.Distance; i++)
            {
                current = instruction.Direction switch
                {
                    Direction.Up => new Vector3(current.X, current.Y + 1, current.Z + 1),
                    Direction.Right => new Vector3(current.X + 1, current.Y, current.Z + 1),
                    Direction.Down => new Vector3(current.X, current.Y - 1, current.Z + 1),
                    Direction.Left => new Vector3(current.X - 1, current.Y, current.Z + 1),
                    _ => throw new InvalidOperationException()
                };

                yield return current;
            }
        }
    }

    private static IEnumerable<Instruction>[] ParseInput(string input)
    {
        static IEnumerable<Instruction> ParseLine(string line) => line
            .Split(',')
            .Select(ins => new Instruction((Direction)ins[0], int.Parse(ins[1..])));

        return input.Lines().Select(ParseLine).ToArray();
    }

    record Instruction(Direction Direction, int Distance);

    private class Vector3ComparerAsVector2 : IEqualityComparer<Vector3>
    {
        public bool Equals(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y;

        public int GetHashCode(Vector3 obj) => HashCode.Combine(obj.X, obj.Y);
    }
}
