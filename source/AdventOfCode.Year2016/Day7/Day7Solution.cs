// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2016, 7, "Internet Protocol Version 7")]
public partial class Day7Solution : IProblemSolver
{
    public object? PartOne(string input) => ParseInput(input).Count(ipv7 => ipv7.SupportsTLS());

    public object? PartTwo(string input) => ParseInput(input).Count(ipv7 => ipv7.SupportsSSL());

    private static IEnumerable<IPv7Address> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var matches = IPv7Regex().Matches(line);
            var succesfulMatches = matches.Where(m => m.Success);
            if (succesfulMatches.Any())
            {
                var address = succesfulMatches.SelectMany(m => m.Groups["address"].Captures.Select(c => c.Value)).ToArray();
                var hypernet = succesfulMatches.SelectMany(m => m.Groups["hypernet"].Captures.Select(c => c.Value)).ToArray();
                yield return new IPv7Address(address, hypernet);
            }
        }
    }

    [GeneratedRegex(@"(?<address>[a-z]+)(?:\[(?<hypernet>[a-z]+)\])?")]
    private static partial Regex IPv7Regex();

    private record class IPv7Address(string[] Address, string[] HypernetSequence)
    {
        public bool SupportsTLS()
        {
            var addressABBA = this.Address.Any(this.ContainsABBA);
            var hypernetABBA = this.HypernetSequence.Any(this.ContainsABBA);
            return addressABBA && !hypernetABBA;
        }

        public bool SupportsSSL()
        {
            foreach (var sequence in this.Address)
            {
                for (var i = 0; i < sequence.Length - 2; i++)
                {
                    if (sequence[i] == sequence[i + 2] && sequence[i] != sequence[i + 1])
                    {
                        var bab = $"{sequence[i + 1]}{sequence[i]}{sequence[i + 1]}";
                        if (this.HypernetSequence.Any(hypernet => hypernet.Contains(bab)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool ContainsABBA(string sequence)
        {
            for (var i = 0; i < sequence.Length - 3; i++)
            {
                if (sequence[i] == sequence[i + 3] && sequence[i + 1] == sequence[i + 2] && sequence[i] != sequence[i + 1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
