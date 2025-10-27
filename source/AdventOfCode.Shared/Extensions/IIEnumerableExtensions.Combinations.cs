// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;

public static partial class IIEnumerableExtensions
{
    /// <summary>
    /// Finds all the combinations of the given values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values">The values to find combinations.</param>
    /// <param name="count">The size of each combination.</param>
    /// <returns>An enumerable containing all the subsequent combinations of the values provided.</returns>
    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> values, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (count == 0)
        {
            yield return [];
            yield break;
        }

        var i = 0;
        foreach (var value in values)
        {
            foreach (var combination in values.Skip(++i).Combinations(count - 1))
            {
                yield return [value, .. combination];
            }
        }
    }
}
