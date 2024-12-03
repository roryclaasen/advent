namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Drawing;

[Problem(2015, 03, "Perfectly Spherical Houses in a Vacuum")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var santa = new Point(0, 0);

        var delivered = new Dictionary<string, int>
        {
            { santa.ToString(), 1 }
        };

        foreach (var dir in input)
        {
            santa = MovePoint(santa, dir);

            if (!delivered.ContainsKey(santa.ToString()))
            {
                delivered[santa.ToString()] = 1;
            }
            else
            {
                delivered[santa.ToString()]++;
            }
        }


        return delivered.Count;
    }

    public object? PartTwo(string input)
    {
        var santa = new Point(0, 0);
        var robot = new Point(0, 0);

        var delivered = new Dictionary<string, int>
        {
            { santa.ToString(), 2 },
        };

        var isSanta = true;

        foreach (var dir in input)
        {
            var point = MovePoint(isSanta ? santa : robot, dir);

            if (!delivered.ContainsKey(point.ToString()))
            {
                delivered[point.ToString()] = 1;
            }
            else
            {
                delivered[point.ToString()]++;
            }

            if (isSanta)
            {
                santa = point;
            }
            else
            {
                robot = point;
            }

            isSanta = !isSanta;
        }

        return delivered.Count;
    }

    private static Point MovePoint(Point point, char dir)
    {
        if (dir == '^')
        {
            point.Y++;
        }
        else if (dir == 'v')
        {
            point.Y--;
        }
        else if (dir == '>')
        {
            point.X++;
        }
        else if (dir == '<')
        {
            point.X--;
        }

        return point;
    }
}
