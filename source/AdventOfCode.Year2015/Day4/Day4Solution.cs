// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Problem;

[Problem(2015, 4, "The Ideal Stocking Stuffer")]
public partial class Day4Solution : IProblemSolver
{
    [SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "MD5 is required by the puzzle and is not used for security.")]
    public object? PartOne(string input)
    {
        int? number = null;
        var inputNumber = 0;
        do
        {
            var secret = $"{input}{inputNumber}";

            var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(secret));
            var hash = MakeHashReadable(hashBytes);
            if (hash.StartsWith("00000", StringComparison.Ordinal))
            {
                number = inputNumber;
            }

            inputNumber++;
        }
        while (number is null);

        return number;
    }

    [SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "MD5 is required by the puzzle and is not used for security.")]
    public object? PartTwo(string input)
    {
        int? number = null;
        var inputNumber = 0;
        do
        {
            var secret = $"{input}{inputNumber}";

            var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(secret));
            var hash = MakeHashReadable(hashBytes);
            if (hash.StartsWith("000000", StringComparison.Ordinal))
            {
                number = inputNumber;
            }

            inputNumber++;
        }
        while (number is null);

        return number;
    }

    private static string MakeHashReadable(byte[] input)
    {
        return Convert.ToHexString(input);
    }
}
