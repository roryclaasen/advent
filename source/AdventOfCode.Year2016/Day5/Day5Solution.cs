// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016;

using AdventOfCode.Problem;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
            if (hashString.StartsWith("00000"))
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
            if (hashString.StartsWith("00000"))
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

    private string GetHash(string input)
    {
        if (this.hashCache.TryGetValue(input, out var hashString))
        {
            return hashString;
        }

        var hash = MD5.HashData(Encoding.ASCII.GetBytes(input));
        hashString = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        this.hashCache.Add(input, hashString);
        return hashString;
    }
}
