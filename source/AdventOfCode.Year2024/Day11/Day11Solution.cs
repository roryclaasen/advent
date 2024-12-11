namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2024, 11, "Plutonian Pebbles")]
public partial class Day11Solution : IProblemSolver
{
    private readonly Dictionary<(long, int), long> stoneCache = [];

    public object? PartOne(string input)
        => ParseInput(input).Sum(s => Calculate(s, 25));

    public object? PartTwo(string input)
        => ParseInput(input).Sum(s => Calculate(s, 75));

    private long Calculate(long stone, int blinks)
    {
        if (blinks == 0)
        {
            return 1;
        }

        var key = (stone, blinks);
        if (this.stoneCache.TryGetValue(key, out var value))
        {
            return value;
        }

        if (stone == 0)
        {
            value = Calculate(1, blinks - 1);
        }
        else
        {
            var digits = stone.CountDigits();
            if (digits % 2 != 0)
            {
                value = Calculate(stone * 2024, blinks - 1);
            }
            else
            {
                var (left, right) = stone.SplitDigits(digits / 2);
                value = Calculate(left, blinks - 1) + Calculate(right, blinks - 1);
            }
        }

        this.stoneCache.Add(key, value);
        return value;
    }

    private static IEnumerable<long> ParseInput(string input) => input.Spaces().Select(long.Parse);
}
