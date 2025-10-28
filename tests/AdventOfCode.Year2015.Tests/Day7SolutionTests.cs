// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015.Tests;

using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Shared.Tests;

[TestClass]
public class Day7SolutionTests : SolverBaseTests<Day7Solution>
{
    [TestMethod]
    public void SolvePart1()
    {
        var solvedAnswer = this.Solver.RunInput(@"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i");

        var expected = new Dictionary<string, ushort>
        {
            { "d", 72 },
            { "e", 507 },
            { "f", 492 },
            { "g", 114 },
            { "h", 65412 },
            { "i", 65079 },
            { "x", 123 },
            { "y", 456 }
        };

        Assert.AreEqual(ToAssertableString(expected), ToAssertableString(solvedAnswer));
    }

    private static string ToAssertableString(IDictionary<string, ushort> dictionary)
    {
        var pairStrings = dictionary.OrderBy(p => p.Key).Select(p => p.Key + ":" + p.Value);
        return string.Join("; ", pairStrings);
    }
}
