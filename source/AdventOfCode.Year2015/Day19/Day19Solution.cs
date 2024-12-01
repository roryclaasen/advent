namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2015, 19, "Medicine for Rudolph")]
public class Day19Solution : ISolver
{
    public object? PartOne(string input)
    {
        var (molecule, replacements) = this.ParseInput(input);

        var molecules = GetAllNewMolecules(molecule, replacements);
        return molecules.Count;
    }

    public object? PartTwo(string input)
    {
        var (moleculeGoal, replacements) = this.ParseInput(input);

        var currentMolecules = new HashSet<string> { moleculeGoal };
        var step = 0;
        while (!currentMolecules.Contains("e"))
        {
            step++;
            currentMolecules = currentMolecules
                .SelectMany(m => GetAllPreviousMolecules(m, replacements))
                .OrderBy(step => step.Length)
                .Take(500)
                .ToHashSet();
        }

        return step;
    }

    private static HashSet<string> GetAllNewMolecules(string molecule, IEnumerable<Replacement> replacements)
    {
        var molecules = new HashSet<string>();
        foreach (var replacement in replacements)
        {
            foreach (var character in molecule.AllIndexesOf(replacement.From))
            {
                var newMolecule = molecule[..character] + replacement.To + molecule[(character + replacement.From.Length)..];
                molecules.Add(newMolecule);
            }
        }

        return molecules;
    }

    private static HashSet<string> GetAllPreviousMolecules(string molecule, IEnumerable<Replacement> replacements)
    {
        var molecules = new HashSet<string>();
        foreach (var replacement in replacements)
        {
            foreach (var character in molecule.AllIndexesOf(replacement.To))
            {
                var newMolecule = molecule[..character] + replacement.From + molecule[(character + replacement.To.Length)..];
                molecules.Add(newMolecule);
            }
        }

        return molecules;
    }

    private (string Molecule, IEnumerable<Replacement> Replacements) ParseInput(string input)
    {
        var parts = input.Lines(2);
        return (parts[1], ParseReplacements(parts[0]));
    }

    private static IEnumerable<Replacement> ParseReplacements(string input)
    {
        foreach (var line in input.Lines())
        {
            var parts = line.Split(" => ");
            yield return new Replacement(parts[0], parts[1]);
        }
    }

    private record Replacement(string From, string To);
}
