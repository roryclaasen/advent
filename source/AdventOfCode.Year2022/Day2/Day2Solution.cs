namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System;
using System.Linq;

[Problem(2022, 2, "Rock Paper Scissors")]
public partial class Day2Solution : ISolver
{
    public object? PartOne(string input) => CalculateScore(ParseInput1(input));

    public object? PartTwo(string input)
    {
        var moves = new[] { RPSMove.Rock, RPSMove.Paper, RPSMove.Scissors };
        return CalculateScore(ParseInput2(input).Select(s =>
        {
            var move = moves.First(m => PlayGame(m, s.Opponent) == s.Result);
            return new RPSRound(move, s.Opponent);
        }));
    }

    private static IEnumerable<RPSRound> ParseInput1(string input)
    {
        foreach (var round in input.Lines().Select(l => l.Split(' ')))
        {
            var opponent = round[0] == "A" ? RPSMove.Rock : round[0] == "B" ? RPSMove.Paper : RPSMove.Scissors;
            var move = round[1] == "X" ? RPSMove.Rock : round[1] == "Y" ? RPSMove.Paper : RPSMove.Scissors;
            yield return new RPSRound(move, opponent);
        }
    }

    private static IEnumerable<Strategy> ParseInput2(string input)
    {
        foreach (var round in input.Lines().Select(l => l.Split(' ')))
        {
            var opponent = round[0] == "A" ? RPSMove.Rock : round[0] == "B" ? RPSMove.Paper : RPSMove.Scissors;
            var move = round[1] == "X" ? Outcome.Lose : round[1] == "Y" ? Outcome.Draw : Outcome.Win;
            yield return new Strategy(move, opponent);
        }
    }

    private static int CalculateScore(IEnumerable<RPSRound> rounds)
    {
        var totalScore = 0;
        foreach (var round in rounds)
        {
            totalScore += (int)round.Move;
            totalScore += PlayGame(round.Move, round.Opponent) switch
            {
                Outcome.Draw => 3,
                Outcome.Win => 6,
                _ => 0
            };
        }

        return totalScore;
    }

    private static Outcome PlayGame(RPSMove a, RPSMove b)
    {
        if (a == b)
        {
            return Outcome.Draw;
        }

        return a switch
        {
            RPSMove.Rock => b == RPSMove.Scissors ? Outcome.Win : Outcome.Lose,
            RPSMove.Paper => b == RPSMove.Rock ? Outcome.Win : Outcome.Lose,
            RPSMove.Scissors => b == RPSMove.Paper ? Outcome.Win : Outcome.Lose,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, null)
        };
    }

    private enum RPSMove : int { Rock = 1, Paper = 2, Scissors = 3 }

    private enum Outcome { Win, Lose, Draw }

    private record RPSRound(RPSMove Move, RPSMove Opponent);

    private record Strategy(Outcome Result, RPSMove Opponent);
}
