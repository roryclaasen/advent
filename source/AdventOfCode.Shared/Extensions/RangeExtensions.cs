// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class RangeExtensions
{
    extension(Range range)
    {
        public IEnumerable<int> ToEnumerable()
        {
            for (int i = range.Start.Value; i <= range.End.Value; i++)
            {
                yield return i;
            }
        }

        public RangeEnumerator<int> GetEnumerator() => new(range.Start.Value, range.End.Value);
    }

    public static Range ToRange(this IEnumerable<int> values)
    {
        if (values.Count() != 2)
        {
            throw new ArgumentException("The input must contain exactly two values.");
        }

        return new Range(values.First(), values.Last());
    }

    public static RangeEnumerator<TNumber> GetEnumerator<TNumber>(this (TNumber Start, TNumber End) range)
        where TNumber : struct, INumber<TNumber>
        => new(range.Start, range.End);

    public static RangeEnumerator<TNumber> GetEnumerator<TNumber>(this Tuple<TNumber, TNumber> range)
        where TNumber : struct, INumber<TNumber>
        => new(range.Item1, range.Item2);
}
