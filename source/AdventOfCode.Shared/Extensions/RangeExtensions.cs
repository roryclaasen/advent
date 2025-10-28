// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

public static class RangeExtensions
{
    /// <summary>
    /// Converts a collection of two integers to a <see cref="Range"/>.
    /// </summary>
    /// <param name="values">The enumerable.</param>
    /// <returns>The range.</returns>
    /// <exception cref="ArgumentException">Throws when <paramref name="values"/> doesn't contain exactly 2 values.</exception>
    public static Range ToRange(this IEnumerable<int> values)
    {
        if (values.Count() != 2)
        {
            throw new ArgumentException("The input must contain exactly two values.");
        }

        return new Range(values.First(), values.Last());
    }

    /// <summary>
    /// Converts a range to an enumerable.
    /// </summary>
    /// <param name="range">The range.</param>
    /// <returns>An enumerable of all the values from the start to the end of the <paramref name="range"/></returns>
    public static IEnumerable<int> ToEnumerable(this Range range)
    {
        for (int i = range.Start.Value; i <= range.End.Value; i++)
        {
            yield return i;
        }
    }
}
