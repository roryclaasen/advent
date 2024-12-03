namespace AdventOfCode.Year2020;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Linq;

[Problem(2020, 3, "Toboggan Trajectory")]
public partial class Day3Solution : IProblemSolver
{
    public object? PartOne(string input) => Navigate([.. input.Lines()], 3, 1);


    public object? PartTwo(string input)
    {
        var map = input.Lines().ToArray();

        long r1d1 = Navigate(map, 1, 1);
        long r3d1 = Navigate(map, 3, 1);
        long r5d1 = Navigate(map, 5, 1);
        long r7d1 = Navigate(map, 7, 1);
        long r1d2 = Navigate(map, 1, 2);
        return r1d1 * r3d1 * r5d1 * r7d1 * r1d2;
    }

    private static int Navigate(string[] map, int xSlope, int ySlope)
    {
        var x = 0;
        var y = 0;
        var trees = 0;

        while (y < map.Length)
        {
            if (map[y][x] == '#')
            {
                trees++;
            }

            x = (x + xSlope) % map[y].Length;
            y += ySlope;
        }

        return trees;
    }
}
