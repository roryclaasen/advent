// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Linq;

[Problem(2016, 9, "Explosives in Cyberspace")]
public partial class Day9Solution : IProblemSolver
{
    public object? PartOne(string input) => ExpandLength(input, 0, input.Length, false);

    public object? PartTwo(string input) => ExpandLength(input, 0, input.Length, true);

    private static long ExpandLength(string input, int index, int length, bool recursive)
    {
        var result = 0L;
        while (index < length)
        {
            if (input[index] == '(')
            {
                var markerEnd = input.IndexOf(')', index + 1);
                var marker = input[(index + 1)..markerEnd];
                var (chars, repeat) = marker.Split('x').Select(int.Parse).ToArray();
                index = markerEnd + 1;
                if (recursive)
                {
                    result += ExpandLength(input, index, index + chars, true) * repeat;
                }
                else
                {
                    result += chars * repeat;
                }

                index += chars;
            }
            else
            {
                result++;
                index++;
            }
        }

        return result;
    }
}
