namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;
using System.Linq;

[Problem(2015, 18, "Like a GIF For Your Yard")]
public class Day18Solution : ISolver
{
    public int GridSize { get; set; } = 100;

    public int Steps { get; set; } = 100;

    public object? PartOne(string input)
    {
        var grid = this.ParseInput(input);

        for (var i = 0; i < this.Steps; i++)
        {
            grid = this.ProcessGrid(grid);
        }

        return grid.OfType<bool>().Count(b => b == true);
    }

    public object? PartTwo(string input)
    {
        var grid = this.ParseInput(input);

        void TurnOnCorners()
        {
            grid[0, 0] = true;
            grid[0, this.GridSize - 1] = true;
            grid[this.GridSize - 1, 0] = true;
            grid[this.GridSize - 1, this.GridSize - 1] = true;
        }

        TurnOnCorners();
        for (var i = 0; i < this.Steps; i++)
        {
            grid = this.ProcessGrid(grid);
            TurnOnCorners();
        }

        return grid.OfType<bool>().Count(b => b == true);
    }

    bool[,] ProcessGrid(bool[,] grid)
    {
        var newGrid = new bool[GridSize, GridSize];

        for (var y = 0; y < this.GridSize; y++)
        {
            for (var x = 0; x < this.GridSize; x++)
            {
                var neighbours = this.CountNeighbours(grid, x, y);

                if (grid[x, y])
                {
                    newGrid[x, y] = neighbours == 2 || neighbours == 3;
                }
                else
                {
                    newGrid[x, y] = neighbours == 3;
                }
            }
        }

        return newGrid;
    }

    int CountNeighbours(bool[,] grid, int x, int y)
    {
        var count = 0;

        for (var dy = -1; dy <= 1; dy++)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                var nx = x + dx;
                var ny = y + dy;

                if (nx < 0 || nx >= GridSize || ny < 0 || ny >= GridSize)
                {
                    continue;
                }

                if (grid[nx, ny])
                {
                    count++;
                }
            }
        }

        return count;
    }

    bool[,] ParseInput(string input)
    {
        var grid = new bool[GridSize, GridSize];

        var lines = input.Lines();

        for (var y = 0; y < GridSize; y++)
        {
            var line = lines[y];

            for (var x = 0; x < GridSize; x++)
            {
                grid[x, y] = line[x] == '#';
            }
        }

        return grid;
    }
}
