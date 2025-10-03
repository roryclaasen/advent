namespace AdventOfCode.Year2015;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2015, 15, "Science for Hungry People")]
public partial class Day15Solution : IProblemSolver
{
    private static int MaxTeaspoons = 100;

    public object? PartOne(string input)
    {
        var ingredients = ParseInput(input);
        return GetBestScore([.. ingredients], (capacity, durability, flavor, texture, _) => capacity * durability * flavor * texture);
    }

    public object? PartTwo(string input)
    {
        var ingredients = ParseInput(input);
        return GetBestScore([.. ingredients], (capacity, durability, flavor, texture, calories) => calories == 500 ? capacity * durability * flavor * texture : 0);
    }

    private static int GetBestScore(Ingredient[] ingredients, Func<int, int, int, int, int, int> bestScore)
    {
        var best = 0;

        for (var i = 0; i <= MaxTeaspoons; i++)
        {
            for (var j = 0; j <= MaxTeaspoons - i; j++)
            {
                for (var k = 0; k <= MaxTeaspoons - i - j; k++)
                {
                    var l = MaxTeaspoons - i - j - k;
                    var capacity = Math.Max(0, ingredients[0].Capacity * i + ingredients[1].Capacity * j + ingredients[2].Capacity * k + ingredients[3].Capacity * l);
                    var durability = Math.Max(0, ingredients[0].Durability * i + ingredients[1].Durability * j + ingredients[2].Durability * k + ingredients[3].Durability * l);
                    var flavor = Math.Max(0, ingredients[0].Flavor * i + ingredients[1].Flavor * j + ingredients[2].Flavor * k + ingredients[3].Flavor * l);
                    var texture = Math.Max(0, ingredients[0].Texture * i + ingredients[1].Texture * j + ingredients[2].Texture * k + ingredients[3].Texture * l);
                    var calories = Math.Max(0, ingredients[0].Calories * i + ingredients[1].Calories * j + ingredients[2].Calories * k + ingredients[3].Calories * l);
                    var score = bestScore(capacity, durability, flavor, texture, calories);
                    best = Math.Max(best, score);
                }
            }
        }

        return best;
    }

    private static IEnumerable<Ingredient> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = IngredientRegex().Match(line);
            if (!match.Success)
            {
                throw new Exception($"Failed to parse line '{line}'");
            }

            var name = match.Groups["name"].Value;
            var capacity = int.Parse(match.Groups["capacity"].Value);
            var durability = int.Parse(match.Groups["durability"].Value);
            var flavor = int.Parse(match.Groups["flavor"].Value);
            var texture = int.Parse(match.Groups["texture"].Value);
            var calories = int.Parse(match.Groups["calories"].Value);

            yield return new Ingredient(name, capacity, durability, flavor, texture, calories);
        }
    }

    private record class Ingredient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories);

    [GeneratedRegex("(?<name>\\w+): capacity (?<capacity>-?\\d+), durability (?<durability>-?\\d+), flavor (?<flavor>-?\\d+), texture (?<texture>-?\\d+), calories (?<calories>-?\\d+)")]
    private static partial Regex IngredientRegex();
}
