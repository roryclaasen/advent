namespace AdventOfCode.Year2022;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2022, 4, "Camp Cleanup")]
public partial class Day4Solution : IProblemSolver
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

    private static IEnumerable<ElfPair> ParseInput(string input)
    {
        foreach (var line in input.Lines())
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

    private record Assignment(int Start, int End);

    private record ElfPair(Assignment Elf1, Assignment Elf2);
}
