namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;
using System;

[Problem(2022, 4, "Camp Cleanup")]
public class Day4Solution : ISolver
{
    public object? PartOne(string input)
    {
        var count = 0;
        foreach (var (Elf1, Elf2) in ParseInput(input))
        {
            var (first, other) = (Elf1.Start == Elf2.Start && Elf1.End >= Elf2.End) || Elf1.Start < Elf2.Start ? (Elf1, Elf2) : (Elf2, Elf1);
            if (first.End >= other.End)
            {
                count++;
            }
        }

        return count;
    }

    public object? PartTwo(string input)
    {
        var count = 0;
        foreach (var (Elf1, Elf2) in ParseInput(input))
        {
            var (first, other) = (Elf1.Start == Elf2.Start && Elf1.End >= Elf2.End) || Elf1.Start < Elf2.Start ? (Elf1, Elf2) : (Elf2, Elf1);
            if (other.Start <= first.End)
            {
                count++;
            }
        }

        return count;
    }

    static IEnumerable<ElfPair> ParseInput(string input)
    {
        foreach (var line in input.SplitNewLine())
        {
            var parts = line
                .Split(',')
                .Select(s => s
                    .Split('-')
                    .Select(int.Parse)
                    .ToArray())
                .Select(s => new Assignment(s[0], s[1]))
                .ToArray();

            yield return new ElfPair(parts[0], parts[1]);
        }
    }

    record Assignment(int Start, int End);

    record ElfPair(Assignment Elf1, Assignment Elf2);
}
