// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2022;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2022, 5, "Supply Stacks")]
public partial class Day5Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var parsedInput = ParseInput(input);
        var stacks = parsedInput.Stacks;
        foreach (var (Count, From, To) in parsedInput.Instructions)
        {
            for (var i = 0; i < Count; i++)
            {
                stacks[To - 1].Push(stacks[From - 1].Pop());
            }
        }

        return string.Join(string.Empty, stacks.Select(s => s.First().ToString()));
    }

    public object? PartTwo(string input)
    {
        var parsedInput = ParseInput(input);
        var stacks = parsedInput.Stacks;
        foreach (var (Count, From, To) in parsedInput.Instructions)
        {
            var items = new Stack<char>();
            for (var i = 0; i < Count; i++)
            {
                items.Push(stacks[From - 1].Pop());
            }

            foreach (var item in items)
            {
                stacks[To - 1].Push(item);
            }
        }

        return string.Join(string.Empty, stacks.Select(s => s.First().ToString()));
    }

    private static IputData ParseInput(string input)
    {
        var data = input.Lines(2);
        return new IputData([.. ParseStack(data[0])], PasrseInstruction(data[1]));
    }

    private static IEnumerable<Stack<char>> ParseStack(string input)
    {
        static string TidyStackItem(string stack) => stack
            .Replace("[", string.Empty)
            .Replace("]", string.Empty)
            .Replace(" ", string.Empty);

        var stackInput = input
            .Lines()
            .Select(l => l.SplitInParts(3, 1).Select(TidyStackItem));
        var stackCount = stackInput.Last().Count();
        var stackData = stackInput.SkipLast(1).Reverse().Select(l => l.ToArray());

        for (var i = 0; i < stackCount; i++)
        {
            var stack = new Stack<char>();
            foreach (var line in stackData)
            {
                if (i >= line.Length)
                {
                    continue;
                }

                var character = line[i];
                if (!string.IsNullOrWhiteSpace(character))
                {
                    stack.Push(character.ToCharArray().First());
                }
            }

            yield return stack;
        }
    }

    private static IEnumerable<Instruction> PasrseInstruction(string input)
    {
        var regexMatch = new Regex(@"^move (\d+) from (\d+) to (\d+)$");

        foreach (var line in input.Lines())
        {
            var temp = regexMatch.Match(line);
            var groups = temp?.Groups;
            if (groups is null)
            {
                throw new ArgumentException($"Invalid input: {line}");
            }

            yield return new Instruction(int.Parse(groups[1].Value), int.Parse(groups[2].Value), int.Parse(groups[3].Value));
        }
    }

    private record IputData(Stack<char>[] Stacks, IEnumerable<Instruction> Instructions);

    private record Instruction(int Count, int From, int To);
}
