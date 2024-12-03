namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Problem(2023, 7, "Camel Cards")]
public partial class Day7Solution : IProblemSolver
{
    public object? PartOne(string input)
        => ParseInput(input)
            .OrderBy(h => h.PairType())
            .ThenBy(c => c, new CamelHandSort())
            .Select((h, i) => h.BidAmount * (i + 1))
            .Sum();

    public object? PartTwo(string input)
        => ParseInput(input)
            .OrderBy(h => h.PairTypeWithJoker())
            .ThenBy(c => c, new CamelHandSort(1))
            .Select((h, i) => h.BidAmount * (i + 1))
            .Sum();

    private static IEnumerable<Hand> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var parts = line.Split(" ");
            var bidAmount = int.Parse(parts[1]);
            var cards = parts[0].ToCharArray();
            yield return new Hand(cards, bidAmount);
        }
    }

    private class CamelHandSort(int JokerValue = 11) : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if (x is null && y is null)
                return 0;
            else if (x is null)
                return -1;
            else if (y is null)
                return 1;

            for (var i = 0; i < x.CamelCards.Length; i++)
            {
                var thisCard = GetCardValue(x.CamelCards[i]);
                var otherCard = GetCardValue(y.CamelCards[i]);

                var result = thisCard.CompareTo(otherCard);
                if (result != 0)
                    return result;
            }

            return 0;
        }

        private int GetCardValue(char card) => card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => JokerValue,
            'T' => 10,
            _ => card - '0'
        };
    }

    [DebuggerDisplay("Hand {string.Join(string.Empty, CamelCards)} {BidAmount}")]
    private record Hand(char[] CamelCards, int BidAmount)
    {
        public int PairType() => GetPairType(this.CamelCards);

        public int PairTypeWithJoker()
        {
            if (!this.CamelCards.Contains('J'))
            {
                return this.PairType();
            }

            static char[] ReplaceJoker(char newCard, char[] cards)
                => cards.Select(c => c == 'J' ? newCard : c).ToArray();

            return this.CamelCards
                .Where(c => c != 'J')
                .Distinct()
                .Select(c => ReplaceJoker(c, this.CamelCards))
                .Append(this.CamelCards)
                .Max(GetPairType);
        }

        private static int GetPairType(char[] Cards)
        {
            var group = Cards.GroupBy(c => c);
            return (group.Count(), group.Max(g => g.Count())) switch
            {
                (5, 1) => 0, // High Card
                (4, 2) => 1, // One Pair
                (3, 2) => 2, // Two Pair
                (3, 3) => 3, // Three of a Kind
                (2, 3) => 4, // Full House
                (2, 4) => 5, // Four of a Kind
                (1, 5) => 6, // Five of a Kind
                _ => throw new Exception("Unable to determine pair type")
            };
        }
    }
}
