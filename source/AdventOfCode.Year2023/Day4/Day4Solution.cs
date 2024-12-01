namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2023, 4, "Scratchcards")]
public partial class Day4Solution : ISolver
{
    public object? PartOne(string input)
    {
        static int CalculatePoints(ScratchCard card)
        {
            if (card.NumbersThatWon.Count <= 0)
            {
                return 0;
            }

            return Enumerable
                .Range(1, card.NumbersThatWon.Count - 1)
                .Aggregate(1, (s, i) => s * 2);
        }

        return ParseInput(input).Sum(CalculatePoints);
    }


    public object? PartTwo(string input)
    {
        var cards = ParseInput(input);
        var totalCount = cards.ToDictionary(c => c.Id, _ => 1);
        foreach (var card in cards)
        {
            var numCards = totalCount[card.Id];
            for (var i = 0; i < card.NumbersThatWon.Count; i++)
            {
                var key = card.Id + 1 + i;
                if (totalCount.ContainsKey(key))
                {
                    totalCount[key] += numCards;
                }
            }
        }

        return totalCount.Values.Sum();
    }

    private static IEnumerable<ScratchCard> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = CardRegex().Match(line);
            if (match.Success)
            {
                var id = int.Parse(match.Groups["Id"].Value);
                var winning = match.Groups["Winning"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                var picked = match.Groups["Picked"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                yield return new ScratchCard(id, winning, picked);
            }
            else
            {
                throw new Exception($"Unable to parse line: {line}");
            }
        }
    }

    private record ScratchCard(int Id, IReadOnlyCollection<int> WinningNumbers, IReadOnlyCollection<int> PickedNumbers)
    {
        private bool IsWinningNumber(int number) => WinningNumbers.Contains(number);

        public IReadOnlyCollection<int> NumbersThatWon => PickedNumbers.Where(IsWinningNumber).ToList();
    }

    [GeneratedRegex(@"Card +(?<Id>\d+):(?<Winning>(?: +\d+)+) \|(?<Picked>(?: +\d+)+)")]
    private static partial Regex CardRegex();
}
