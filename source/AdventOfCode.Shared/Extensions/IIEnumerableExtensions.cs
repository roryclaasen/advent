namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;

public static class IIEnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int count)
        => count == 0
            ? new[] { Array.Empty<T>() }
            : elements.SelectMany((e, i) => elements.Skip(i + 1).Combinations(count - 1).Select(c => (new[] { e }).Concat(c)));
}
