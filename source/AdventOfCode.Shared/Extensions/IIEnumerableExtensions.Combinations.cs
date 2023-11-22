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
    /// <returns>An enumerable contianing all the subsequent combinations of the values provided.</returns>
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> values, int count)
        => count == 0
            ? new[] { Array.Empty<T>() }
            : values.SelectMany((e, i) => values.Skip(i + 1).Combinations(count - 1).Select(c => (new[] { e }).Concat(c)));
}
