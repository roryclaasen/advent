// ------------------------------------------------------------------------------
// <copyright file="Day5Challenge.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure;

    public class Day5Challenge : IChallenge
    {
        private const string Vowels = "aeiou";

        public Task<string> SolvePart1(string input)
        {
            var nice = 0;

            foreach (var line in input.Split(Environment.NewLine))
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

            return this.Answer(nice);
        }

        public Task<string> SolvePart2(string input)
        {
            var nice = 0;

            foreach (var line in input.Split(Environment.NewLine))
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

            return this.Answer(nice);
        }
    }
}
