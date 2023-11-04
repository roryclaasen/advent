namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System;
using System.Linq;

[Problem(2022, 3, "Rucksack Reorganization")]
public class Day3Solution : ISolver
{
    public object? PartOne(string input)
        => ParseInput(input)
            .Select(rucksack => rucksack.Compartment1
            .Distinct()
            .Where(letter => rucksack.Compartment2.Contains(letter))
            .Select(Priority)
            .Sum()
        ).Sum();

    public object? PartTwo(string input)
    {
        var elves = ParseInput(input).ToArray();
        var score = 0;
        for (var i = 0; i < elves.Length; i += 3)
        {
            score += Priority(elves[i].Everything
                .Distinct()
                .Where(letter => elves[i + 1].Everything.Contains(letter) && elves[i + 2].Everything.Contains(letter))
                .Single());
        }

        return score;
    }

    static IEnumerable<Rucksack> ParseInput(string input)
    {
        foreach (var line in input.SplitNewLine())
        {
            var mid = line.Length / 2;
            yield return new Rucksack(line, line.Substring(0, mid), line.Substring(mid, mid));
        }
    }

    static int Priority(char item)
    {
        var id = Convert.ToInt32(item);
        return id >= 97 ? id - 96 : id - 38;
    }

    record Rucksack(string Everything, string Compartment1, string Compartment2);
}
