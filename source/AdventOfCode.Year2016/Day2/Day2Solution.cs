namespace AdventOfCode.Year2016;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Problem(2016, 2, "Bathroom Security")]
public partial class Day2Solution : ISolver
{
    public object? PartOne(string input)
    {
        var keypad = new char[3, 3]
        {
            { '1', '2', '3' },
            { '4', '5', '6' },
            { '7', '8', '9' }
        };

        var sb = new StringBuilder();

        var x = 1;
        var y = 1;
        foreach (var line in ParseInput(input))
        {
            foreach (var direction in line)
            {
                switch (direction)
                {
                    case Direction.Up:
                        y = Math.Max(0, y - 1);
                        break;
                    case Direction.Right:
                        x = Math.Min(2, x + 1);
                        break;
                    case Direction.Down:
                        y = Math.Min(2, y + 1);
                        break;
                    case Direction.Left:
                        x = Math.Max(0, x - 1);
                        break;
                }
            }

            sb.Append(keypad[y, x]);
        }

        return sb.ToString();
    }

    public object? PartTwo(string input)
    {
        var keypad = new char?[5, 5]
        {
            { null, null, '1', null, null},
            { null, '2', '3', '4', null },
            { '5', '6', '7', '8', '9' },
            { null, 'A', 'B', 'C', null },
            { null, null, 'D', null, null},
        };

        var sb = new StringBuilder();

        var x = 0;
        var y = 3;
        foreach (var line in ParseInput(input))
        {
            foreach (var direction in line)
            {
                var nextX = x;
                var nextY = y;
                switch (direction)
                {
                    case Direction.Up:
                        nextY = Math.Max(0, y - 1);
                        break;
                    case Direction.Right:
                        nextX = Math.Min(4, x + 1);
                        break;
                    case Direction.Down:
                        nextY = Math.Min(4, y + 1);
                        break;
                    case Direction.Left:
                        nextX = Math.Max(0, x - 1);
                        break;
                }

                if (keypad[nextY, nextX] != null)
                {
                    x = nextX;
                    y = nextY;
                }
            }

            sb.Append(keypad[y, x]);
        }

        return sb.ToString();
    }

    private static IEnumerable<IEnumerable<Direction>> ParseInput(string input)
        => input.Lines().Select(l => l.Select(c => (Direction)c));
}
