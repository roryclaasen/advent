namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Numerics;

[Problem(2022, 9, "Rope Bridge")]
public partial class Day9Solution : ISolver
{
    public object? PartOne(string input) => GetTailMovements(2, ParseInput(input));

    public object? PartTwo(string input) => GetTailMovements(10, ParseInput(input));

    private static IEnumerable<Instruction> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var parts = line.Split(' ');
            yield return new Instruction((Direction)parts[0].ToCharArray()[0], int.Parse(parts[1]));
        }
    }

    private static int GetTailMovements(int numberOfKnots, IEnumerable<Instruction> instructions)
    {
        Vector2[] knots = new Vector2[numberOfKnots];

        var tailPositions = new List<Vector2> { Vector2.Zero };
        foreach (var (direction, steps) in instructions)
        {
            for (var i = 0; i < steps; i++)
            {
                switch (direction)
                {
                    case Direction.Up:
                        knots[0] += Vector2.UnitY;
                        break;
                    case Direction.Down:
                        knots[0] -= Vector2.UnitY;
                        break;
                    case Direction.Left:
                        knots[0] -= Vector2.UnitX;
                        break;
                    case Direction.Right:
                        knots[0] += Vector2.UnitX;
                        break;
                    default:
                        throw new Exception("Unknown direction");
                }

                for (var k = 1; k < knots.Length; k++)
                {
                    var head = knots[k - 1];
                    var tail = knots[k];

                    if (Vector2.Distance(head, tail) < 2)
                    {
                        continue;
                    }

                    knots[k] = MoveKnot(head, tail);
                }

                tailPositions.Add(knots.Last());
            }
        }

        return tailPositions.Distinct().Count();
    }

    private static Vector2 MoveKnot(Vector2 head, Vector2 tail)
    {
        if (head.X != tail.X)
        {
            if (head.X > tail.X)
            {
                tail += Vector2.UnitX;
            }
            else
            {
                tail -= Vector2.UnitX;
            }
        }

        if (head.Y != tail.Y)
        {
            if (head.Y > tail.Y)
            {
                tail += Vector2.UnitY;
            }
            else
            {
                tail -= Vector2.UnitY;
            }
        }

        return tail;
    }

    private record Instruction(Direction Direction, int Steps);
}
