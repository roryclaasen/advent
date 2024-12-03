namespace AdventOfCode.Year2020;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2020, 2, "Password Philosophy")]
public partial class Day2Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var policies = ParseInput(input);
        return policies.Count(p => p.Password.Count(c => c == p.Character) >= p.Min && p.Password.Count(c => c == p.Character) <= p.Max);
    }

    public object? PartTwo(string input)
    {
        var policies = ParseInput(input);
        return policies.Count(p => p.Password[p.Min - 1] == p.Character ^ p.Password[p.Max - 1] == p.Character);
    }

    private static IEnumerable<PasswordPolicy> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = PasswordPolicyRegex().Match(line);
            if (match.Success)
            {
                var min = int.Parse(match.Groups["Min"].Value);
                var max = int.Parse(match.Groups["Max"].Value);
                var character = match.Groups["Character"].Value[0];
                var password = match.Groups["Password"].Value;

                yield return new PasswordPolicy(min, max, character, password);
            }
        }
    }

    private record PasswordPolicy(int Min, int Max, char Character, string Password);

    [GeneratedRegex("(?<Min>\\d+)-(?<Max>\\d+) (?<Character>\\w): (?<Password>\\w+)")]
    private static partial Regex PasswordPolicyRegex();
}
