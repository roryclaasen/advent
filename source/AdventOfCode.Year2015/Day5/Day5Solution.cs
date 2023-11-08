namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;
using System.Text.RegularExpressions;

[Problem(2015, 5, "Doesn't He Have Intern-Elves For This?")]
public class Day5Solution : ISolver
{
    private const string Vowels = "aeiou";

    public object? PartOne(string input)
    {
        var nice = 0;

        foreach (var line in input.Lines())
        {
            if (line.Contains("ab") || line.Contains("cd") || line.Contains("pq") || line.Contains("xy"))
            {
                continue;
            }

            var vowelCount = 0;
            var doubleLetter = false;
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (Vowels.Contains(c))
                {
                    vowelCount++;
                }

                if (i < line.Length - 1 && c == line[i + 1])
                {
                    doubleLetter = true;
                }
            }

            if (vowelCount < 3)
            {
                continue;
            }

            if (doubleLetter)
            {
                nice++;
            }
        }

        return nice;
    }

    public object? PartTwo(string input)
    {
        var nice = 0;

        foreach (var line in input.Lines())
        {
            var doublePair = false;
            var repeats = false;

            for (var i = 0; i < line.Length; i++)
            {
                if (!doublePair && i < line.Length - 1)
                {
                    var doubleChar = new Regex($"{line[i]}{line[i + 1]}");
                    var newLine = doubleChar.Replace(line, "@@", 1);
                    if (doubleChar.IsMatch(newLine))
                    {
                        doublePair = true;
                    }
                }

                if (!repeats && i < line.Length - 2)
                {
                    if (line[i] == line[i + 2])
                    {
                        repeats = true;
                    }
                }

                if (doublePair && repeats)
                {
                    nice++;
                    break;
                }
            }
        }

        return nice;
    }
}
