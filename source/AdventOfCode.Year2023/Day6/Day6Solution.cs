namespace AdventOfCode.Year2023;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2023, 6, "Wait For It")]
public partial class Day6Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var lines = input
            .Lines()
            .Select(s => s.Split(":").Last().Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        if (lines.Length != 2)
        {
            throw new Exception("Invalid input");
        }

        var winningRanges = new List<long>();
        for (var i = 0; i < lines[0].Length; i++)
        {
            var possibleWins = NumberOfWinsPossible(int.Parse(lines[0][i]), int.Parse(lines[1][i]));
            winningRanges.Add(possibleWins);
        }

        return winningRanges.Product();
    }

    public object? PartTwo(string input)
    {
        var (time, distance) = input
            .Lines()
            .Select(s => s.Split(":").Last().Replace(" ", string.Empty))
            .Select(long.Parse)
            .ToArray();

        return NumberOfWinsPossible(time, distance);
    }

    private long NumberOfWinsPossible(long time, long distance)
    {
        var result = FindStartAndEnd(time, distance);
        if (!result.HasValue)
        {
            return 0;
        }

        return (result.Value.End + 1) - result.Value.Start;
    }

    private (long Start, long End)? FindStartAndEnd(long time, long distance)
    {
        var attemptedTimes = new Dictionary<long, bool>();

        bool GetResult(long current)
        {
            if (!attemptedTimes!.TryGetValue(current, out var result))
            {
                result = current * (time - current) > distance;
                attemptedTimes.Add(current, result);
            }

            return result;
        }
        ;

        var start = Search(time, (current) =>
        {
            var currentResult = GetResult(current);
            if (currentResult)
            {
                if (!GetResult(current - 1))
                {
                    return 0;
                }

                return 1;
            }

            return -1;
        });
        if (start == -1)
        {
            return null;
        }

        var end = Search(time, (current) =>
        {
            var currentResult = GetResult(current);
            if (currentResult)
            {
                if (!GetResult(current + 1))
                {
                    return 0;
                }

                return -1;
            }

            return 1;
        });
        if (end == -1)
        {
            return null;
        }

        return (start, end);
    }

    private static long Search(long time, Func<long, int> comparer)
    {
        long high, low, mid;
        high = time - 1;
        low = 1;
        while (low <= high)
        {
            mid = (high + low) / 2;
            var comparison = comparer(mid);
            if (comparison == 0)
            {
                return mid;
            }
            else if (comparison > 0)
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }

        return -1;
    }
}
