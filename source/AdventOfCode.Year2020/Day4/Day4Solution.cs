namespace AdventOfCode.Year2020;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

[Problem(2020, 4, "Passport Processing")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input) => ParseInput(input).Count(p => p.HasNonNullValues());

    private static readonly string[] ValidEyeColors = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];

    public object? PartTwo(string input)
    {
        var valid = 0;
        foreach (var passport in ParseInput(input))
        {
            if (!passport.HasNonNullValues())
            {
                continue;
            }

            if (passport.BirthYear.Length != 4 || !int.TryParse(passport.BirthYear, out var birthYear) || birthYear < 1920 || birthYear > 2002)
            {
                continue;
            }

            if (passport.IssueYear.Length != 4 || !int.TryParse(passport.IssueYear, out var issueYear) || issueYear < 2010 || issueYear > 2020)
            {
                continue;
            }

            if (passport.ExpirationYear.Length != 4 || !int.TryParse(passport.ExpirationYear, out var expirationYear) || expirationYear < 2020 || expirationYear > 2030)
            {
                continue;
            }

            if (passport.Height.EndsWith("cm"))
            {
                var height = passport.Height.Substring(0, passport.Height.Length - 2);
                if (!int.TryParse(height, out var heightValue) || heightValue < 150 || heightValue > 193)
                {
                    continue;
                }
            }
            else if (passport.Height.EndsWith("in"))
            {
                var height = passport.Height.Substring(0, passport.Height.Length - 2);
                if (!int.TryParse(height, out var heightValue) || heightValue < 59 || heightValue > 76)
                {
                    continue;
                }
            }
            else
            {
                continue;
            }

            if (passport.HairColor.Length != 7 || passport.HairColor[0] != '#' || !passport.HairColor.Substring(1).All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')))
            {
                continue;
            }

            if (!ValidEyeColors.Contains(passport.EyeColor))
            {
                continue;
            }

            if (passport.PassportId.Length != 9 || !passport.PassportId.All(char.IsDigit))
            {
                continue;
            }

            valid++;
        }

        return valid;
    }

    private static IEnumerable<Passport> ParseInput(string input)
    {
        foreach (var passportInput in input.Lines(2))
        {
            var parts = passportInput
                .Replace(Environment.NewLine, " ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(':'))
                .ToDictionary(s => s[0], s => s[1]);

            yield return new Passport
            {
                BirthYear = parts.GetValueOrDefault("byr"),
                IssueYear = parts.GetValueOrDefault("iyr"),
                ExpirationYear = parts.GetValueOrDefault("eyr"),
                Height = parts.GetValueOrDefault("hgt"),
                HairColor = parts.GetValueOrDefault("hcl"),
                EyeColor = parts.GetValueOrDefault("ecl"),
                PassportId = parts.GetValueOrDefault("pid"),
                CountryId = parts.GetValueOrDefault("cid")
            };
        }
    }

    private record Passport
    {
        public string? BirthYear { get; init; }
        public string? IssueYear { get; init; }
        public string? ExpirationYear { get; init; }
        public string? Height { get; init; }
        public string? HairColor { get; init; }
        public string? EyeColor { get; init; }
        public string? PassportId { get; init; }
        public string? CountryId { get; init; }

        [MemberNotNullWhen(true, nameof(BirthYear), nameof(IssueYear), nameof(ExpirationYear), nameof(Height), nameof(HairColor), nameof(EyeColor), nameof(PassportId))]
        public bool HasNonNullValues()
        {
            if (this.BirthYear is null) return false;
            if (this.IssueYear is null)  return false;
            if (this.ExpirationYear is null) return false;
            if (this.Height is null) return false;
            if (this.HairColor is null) return false;
            if (this.EyeColor is null) return false;
            if (this.PassportId is null) return false;
            return true;
        }
    }
}
