// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;

public static class ArrayExtensions
{
    public static void Deconstruct<T>(this T[] array, out T item1)
    {
        ArgumentNullException.ThrowIfNull(array, nameof(array));
        if (array.Length < 1)
        {
            throw new ArgumentException("Array must have at least one element.", nameof(array));
        }
        item1 = array[0];
    }

    public static void Deconstruct<T>(this T[] array, out T item1, out T item2)
    {
        ArgumentNullException.ThrowIfNull(array, nameof(array));
        if (array.Length < 2)
        {
            throw new ArgumentException("Array must have at least two elements.", nameof(array));
        }
        item1 = array[0];
        item2 = array[1];
    }

    public static void Deconstruct<T>(this T[] array, out T item1, out T item2, out T item3)
    {
        ArgumentNullException.ThrowIfNull(array, nameof(array));
        if (array.Length < 3)
        {
            throw new ArgumentException("Array must have at least three elements.", nameof(array));
        }
        item1 = array[0];
        item2 = array[1];
        item3 = array[2];
    }
}
