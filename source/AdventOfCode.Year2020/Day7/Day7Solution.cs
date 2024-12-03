namespace AdventOfCode.Year2020;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2020, 7, "Handy Haversacks")]
public partial class Day7Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var bagRules = ParseInput(input).ToList();

        var bags = new HashSet<string>();
        var queue = new Queue<string>();
        queue.Enqueue("shiny gold");

        while (queue.Count > 0)
        {
            var color = queue.Dequeue();
            foreach (var bag in bagRules.Where(b => b.Bags.Any(b => b.Bag == color)))
            {
                if (bags.Add(bag.Color))
                {
                    queue.Enqueue(bag.Color);
                }
            }
        }

        return bags.Count;
    }

    public object? PartTwo(string input)
    {
        var bagRules = ParseInput(input);

        int CountBags(string color)
        {
            var bag = bagRules.First(b => b.Color == color);
            return bag.Bags.Sum(b => b.Count + b.Count * CountBags(b.Bag));
        }


        return CountBags("shiny gold");
    }

    private static IEnumerable<Bag> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var split = line.Split(" bags contain ");
            var color = split[0];
            var bags = split[1]
                .Split(", ")
                .Select(s => s.Replace(" bags", string.Empty).Replace(" bag", string.Empty).Replace(".", string.Empty))
                .Select(r => r.Split(" ", 2))
                .Where(r => r[0] != "no")
                .Select(r => (int.Parse(r[0]), r[1]))
                .ToList();
            yield return new Bag(color, bags);
        }
    }

    private record class Bag(string Color, List<(int Count, string Bag)> Bags);
}
