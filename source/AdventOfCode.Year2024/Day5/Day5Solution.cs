namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

[Problem(2024, 5, "Print Queue")]
public partial class Day5Solution : IProblemSolver
{
    // Caching "input.Rules.Where(r => r.After == page)" as it can save around ~200ms
    private readonly Dictionary<int, PageOrderingRule[]> SearchCache = [];

    public object? PartOne(string input)
        => AddMiddleNumbers(this.GetOrderValidity(ParseInput(input)).Valid);

    public object? PartTwo(string input)
    {
        var inputData = ParseInput(input);
        return AddMiddleNumbers(this.FixOrder(inputData, this.GetOrderValidity(inputData).Invalid));
    }

    private IEnumerable<int[]> FixOrder(InputData input, IEnumerable<int[]> orders)
    {
        foreach (var order in orders)
        {
            var newOrder = new int[order.Length];

            foreach (var page in order)
            {
                if (!this.SearchCache.TryGetValue(page, out var rules))
                {
                    rules = [.. input.Rules.Where(r => r.After == page)];
                    this.SearchCache[page] = rules;
                }

                var newIndex = rules.Count(r => order.Contains(r.Before));
                newOrder[newIndex] = page;
            }

            yield return newOrder;
        }
    }

    private (IEnumerable<int[]> Valid, IEnumerable<int[]> Invalid) GetOrderValidity(InputData input)
    {
        var valid = new List<int[]>();
        var invalid = new List<int[]>();

        foreach (var order in input.Ordering)
        {
            var printedPages = new List<int>();
            foreach (var page in order)
            {
                if (!this.SearchCache.TryGetValue(page, out var rules))
                {
                    rules = [.. input.Rules.Where(r => r.After == page)];
                    this.SearchCache[page] = rules;
                }

                var canPrintPage = !rules.Any(r => order.Contains(r.Before) && !printedPages.Contains(r.Before));
                if (canPrintPage)
                {
                    printedPages.Add(page);
                }
            }

            if (printedPages.Count == order.Length)
            {
                valid.Add(order);
            }
            else
            {
                invalid.Add(order);
            }
        }

        return (valid, invalid);
    }

    private static int AddMiddleNumbers(IEnumerable<int[]> numbers)
        => numbers.Sum(r => r[(int)Math.Floor(r.Length / 2.0)]);

    private static InputData ParseInput(string input)
    {
        var inputInParts = input.Lines(2);
        var rules = inputInParts
            .First()
            .Lines()
            .Select(l => l.Split('|', 2).Select(int.Parse).ToArray())
            .Select(r => new PageOrderingRule(r[0], r[1]));
        var ordering = inputInParts
            .Last()
            .Lines()
            .Select(l => l.Split(',').Select(int.Parse).ToArray());
        return new(rules, ordering);
    }

    private record struct InputData(IEnumerable<PageOrderingRule> Rules, IEnumerable<int[]> Ordering);

    private record struct PageOrderingRule(int Before, int After);
}
