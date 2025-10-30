// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;

public static partial class IIEnumerableExtensions
{
    /// <summary>
    /// Finds all permutations of the given values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values">The values to permutate.</param>
    /// <param name="start">Optional starting value for all permutations</param>
    /// <param name="end">Optional ending value for all permutations</param>
    /// <returns>A list of all the possible permutations of the provided values.</returns>
    public static List<List<T>> Permutations<T>(this IEnumerable<T> values, T? start = null, T? end = null) where T : class
    {
        var permutations = new List<List<T>>();
        var visited = new HashSet<T>();
        var permutation = new List<T>();
        if (start is not null)
        {
            visited.Add(start);
            permutation.Add(start);
        }

        Permute(values, visited, permutation, permutations, end);
        return permutations;
    }

    private static void Permute<T>(IEnumerable<T> values, HashSet<T> visited, List<T> permutation, List<List<T>> permutations, T? end = null) where T : class
    {
        if (permutation.Count == values.Count() && (end is null || permutation.Last()!.Equals(end)))
        {
            permutations.Add(permutation);
            return;
        }

        foreach (var value in values)
        {
            if (!visited.Contains(value))
            {
                visited.Add(value);
                var newPermutation = new List<T>(permutation) { value };
                Permute(values, visited, newPermutation, permutations, end);
                visited.Remove(value);
            }
        }
    }
}
