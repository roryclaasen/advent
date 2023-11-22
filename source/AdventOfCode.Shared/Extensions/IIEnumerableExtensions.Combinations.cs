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
    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> values, int count)
    {
        if (count == 0)
        {
            yield return Array.Empty<T>();
            yield break;
        }

        var i = 0;
        foreach (var value in values)
        {
            foreach (var combination in values.Skip(++i).Combinations(count - 1))
            {
                yield return new[] { value }.Concat(combination).ToArray();
            }
        }
    }
}
