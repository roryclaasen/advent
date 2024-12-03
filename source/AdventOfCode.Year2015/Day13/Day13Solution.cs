namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2015, 13, "Knights of the Dinner Table")]
public partial class Day13Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        return this.GetOptimalHappiness(ParseInput(input));
    }

    public object? PartTwo(string input)
    {
        var guestArrangements = ParseInput(input).ToList();
        var guests = guestArrangements.Select(x => x.Guest).Distinct().ToList();
        foreach (var guest in guests)
        {
            guestArrangements.Add(new GuestArrangement("Me", guest, 0));
            guestArrangements.Add(new GuestArrangement(guest, "Me", 0));
        }

        return this.GetOptimalHappiness(guestArrangements);
    }

    private int GetOptimalHappiness(IEnumerable<GuestArrangement> guestArrangements)
    {
        var permutations = guestArrangements
            .Select(x => x.Guest)
            .Distinct()
            .Permutations();
        var totalHappiness = permutations
            .Select(x => GetTotalHappiness(x, guestArrangements))
            .ToList();

        return totalHappiness.Max();
    }

    private static int GetTotalHappiness(IEnumerable<string> guests, IEnumerable<GuestArrangement> guestArrangements)
    {
        var totalHappiness = 0;
        var totalGuests = guests.Count();
        for (var i = 0; i < totalGuests; i++)
        {
            var guest = guests.ElementAt(i);
            var neighbor = guests.ElementAt(i + 1 == totalGuests ? 0 : i + 1);
            var guestArrangement = guestArrangements.Single(x => x.Guest == guest && x.Neighbor == neighbor).Happiness;
            var neighborArrangement = guestArrangements.Single(x => x.Guest == neighbor && x.Neighbor == guest).Happiness;
            totalHappiness += guestArrangement + neighborArrangement;
        }
        return totalHappiness;
    }

    private static IEnumerable<GuestArrangement> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = GuestArrangementRegex().Match(line);
            if (match.Success)
            {
                var guest = match.Groups["Guest"].Value;
                var neighbor = match.Groups["Neighbor"].Value;
                var happiness = int.Parse(match.Groups["happiness"].Value);
                if (match.Groups["gain"].Value == "lose")
                {
                    happiness *= -1;
                }
                yield return new GuestArrangement(guest, neighbor, happiness);
            }
        }
    }

    private record GuestArrangement(string Guest, string Neighbor, int Happiness);

    [GeneratedRegex("(?<Guest>\\w+) would (?<gain>lose|gain) (?<happiness>\\d+) happiness units by sitting next to (?<Neighbor>\\w+)\\.")]
    private static partial Regex GuestArrangementRegex();
}
