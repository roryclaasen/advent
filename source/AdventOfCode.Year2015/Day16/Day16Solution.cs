namespace AdventOfCode.Year2015;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[Problem(2015, 16, "Aunt Sue")]
public partial class Day16Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var aunts = ParseInput(input);
        foreach (var aunt in aunts)
        {
            if (aunt.Properties.TryGetValue("children", out var children) && children != 3)
                continue;

            if (aunt.Properties.TryGetValue("cats", out var cats) && cats != 7)
                continue;

            if (aunt.Properties.TryGetValue("samoyeds", out var samoyeds) && samoyeds != 2)
                continue;

            if (aunt.Properties.TryGetValue("pomeranians", out var pomeranians) && pomeranians != 3)
                continue;

            if (aunt.Properties.TryGetValue("akitas", out var akitas) && akitas != 0)
                continue;

            if (aunt.Properties.TryGetValue("vizslas", out var vizslas) && vizslas != 0)
                continue;

            if (aunt.Properties.TryGetValue("goldfish", out var goldfish) && goldfish != 5)
                continue;

            if (aunt.Properties.TryGetValue("trees", out var trees) && trees != 3)
                continue;

            if (aunt.Properties.TryGetValue("cars", out var cars) && cars != 2)
                continue;

            if (aunt.Properties.TryGetValue("perfumes", out var perfumes) && perfumes != 1)
                continue;

            return aunt.Number;
        }

        throw new Exception("No aunt found");
    }

    public object? PartTwo(string input)
    {
        var aunts = ParseInput(input);
        foreach (var aunt in aunts)
        {
            if (aunt.Properties.TryGetValue("children", out var children) && children != 3)
                continue;

            if (aunt.Properties.TryGetValue("cats", out var cats) && cats < 7)
                continue;

            if (aunt.Properties.TryGetValue("samoyeds", out var samoyeds) && samoyeds != 2)
                continue;

            if (aunt.Properties.TryGetValue("pomeranians", out var pomeranians) && pomeranians > 3)
                continue;

            if (aunt.Properties.TryGetValue("akitas", out var akitas) && akitas != 0)
                continue;

            if (aunt.Properties.TryGetValue("vizslas", out var vizslas) && vizslas != 0)
                continue;

            if (aunt.Properties.TryGetValue("goldfish", out var goldfish) && goldfish > 5)
                continue;

            if (aunt.Properties.TryGetValue("trees", out var trees) && trees < 3)
                continue;

            if (aunt.Properties.TryGetValue("cars", out var cars) && cars != 2)
                continue;

            if (aunt.Properties.TryGetValue("perfumes", out var perfumes) && perfumes != 1)
                continue;

            return aunt.Number;
        }

        throw new Exception("No aunt found");
    }

    private static IEnumerable<AuntSue> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = AuntRegex().Match(line);
            var number = int.Parse(match.Groups[1].Value);
            var propertiesString = match.Groups[2].Value;

            var properties = new Dictionary<string, int>();
            foreach (var property in propertiesString.Split(","))
            {
                var propertySplits = property.Split(":");
                properties.Add(propertySplits[0].Trim(), int.Parse(propertySplits[1]));
            }
            yield return new AuntSue(number, properties);
        }
    }

    private record AuntSue(int Number, Dictionary<string, int> Properties);

    [GeneratedRegex("Sue (\\d+): (.*)")]
    private static partial Regex AuntRegex();
}
