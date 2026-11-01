// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Problem;

[Problem(2016, 5, "How About a Nice Game of Chess?")]
public partial class Day5Solution : IProblemSolver
{
    private readonly Dictionary<string, string> hashCache = [];

    public object? PartOne(string input)
    {
        var password = new StringBuilder();
        var index = 0;
        while (password.Length < 8)
        {
            var hashString = this.GetHash($"{input}{index}");
            if (hashString.StartsWith("00000", StringComparison.Ordinal))
            {
                password.Append(hashString[5]);
            }

            index++;
        }

        return password.ToString();
    }

    public object? PartTwo(string input)
    {
        var index = 0;
        var found = 0;
        var chars = new char[8];
        while (found < 8)
        {
            var hashString = this.GetHash($"{input}{index}");
            if (hashString.StartsWith("00000", StringComparison.Ordinal))
            {
                var position = hashString[5] - '0';
                if (position < 8 && chars[position] == '\0')
                {
                    chars[position] = hashString[6];
                    found++;
                }
            }

            index++;
        }

        return new string(chars);
    }

    [SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "MD5 is required by the puzzle and is not used for security.")]
    private string GetHash(string input)
    {
        if (this.hashCache.TryGetValue(input, out var hashString))
        {
            return hashString;
        }

        var hash = MD5.HashData(Encoding.ASCII.GetBytes(input));
        hashString = Convert.ToHexStringLower(hash);
        this.hashCache.Add(input, hashString);
        return hashString;
    }
}
