namespace AdventOfCode.Year2015;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;

[Problem(2015, 6, "Probably a Fire Hazard")]
public partial class Day6Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var grid = MakeGrid();
        foreach (var line in input.Lines())
        {
            var instructions = line.Split(' ');
            var isToggle = instructions[0] == "toggle";
            var action = isToggle ? "toggle" : instructions[1];
            var start = instructions[isToggle ? 1 : 2].ToVector2();
            var end = instructions[isToggle ? 3 : 4].ToVector2();

            for (var x = (int)start.X; x <= end.X; x++)
            {
                for (var y = (int)start.Y; y <= end.Y; y++)
                {
                    if (isToggle)
                    {
                        grid[x, y] = grid[x, y] == 1 ? 0 : 1;
                    }
                    else if (action == "on")
                    {
                        grid[x, y] = 1;
                    }
                    else
                    {
                        grid[x, y] = 0;
                    }
                }
            }
        }

        return grid.Count(l => l == 1);
    }

    public object? PartTwo(string input)
    {
        var grid = MakeGrid();
        foreach (var line in input.Lines())
        {
            var instructions = line.Split(' ');
            var isToggle = instructions[0] == "toggle";
            var action = isToggle ? "toggle" : instructions[1];
            var start = instructions[isToggle ? 1 : 2].ToVector2();
            var end = instructions[isToggle ? 3 : 4].ToVector2();

            for (var x = (int)start.X; x <= end.X; x++)
            {
                for (var y = (int)start.Y; y <= end.Y; y++)
                {
                    if (isToggle)
                    {
                        grid[x, y] += 2;
                    }
                    else if (action == "on")
                    {
                        grid[x, y]++;
                    }
                    else if (grid[x, y] > 0)
                    {
                        grid[x, y]--;
                    }
                }
            }
        }

        var brightness = 0;
        for (var x = 0; x < 1000; x++)
        {
            for (var y = 0; y < 1000; y++)
            {
                brightness += grid[x, y];
            }
        }

        return brightness;
    }


    private static int[,] MakeGrid() => new int[1000, 1000];
}
