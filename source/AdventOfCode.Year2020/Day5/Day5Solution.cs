namespace AdventOfCode.Year2020;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2020, 5, "Binary Boarding")]
public class Day5Solution : ISolver
{
    public object? PartOne(string input)
    {
        var passes = this.ParseInput(input);
        return passes.Max(p => p.SeatId);
    }

    public object? PartTwo(string input)
    {
        var passes = this.ParseInput(input);
        var seatIds = passes.Select(p => p.SeatId).OrderBy(id => id).ToArray();
        for (var i = 0; i < seatIds.Length - 1; i++)
        {
            if (seatIds[i + 1] - seatIds[i] == 2)
            {
                return seatIds[i] + 1;
            }
        }

        throw new Exception("Seat not found");
    }

    IEnumerable<BoardingPass> ParseInput(string input)
    {
        foreach (var line in input.Split(Environment.NewLine))
        {
            yield return BoardingPass.Parse(line);
        }
    }

    record BoardingPass(int Row, int Column, int SeatId)
    {
        public static BoardingPass Parse(string input)
        {
            var row = Convert.ToInt32(input[..7].Replace('F', '0').Replace('B', '1'), 2);
            var column = Convert.ToInt32(input[7..].Replace('L', '0').Replace('R', '1'), 2);
            var seatId = row * 8 + column;
            return new BoardingPass(row, column, seatId);
        }
    }
}
