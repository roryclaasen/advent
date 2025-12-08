// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Numerics;

public readonly record struct SimpleRange<TNumber>
    where TNumber : struct, INumber<TNumber>
{
    public SimpleRange(TNumber start, TNumber end)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, end, nameof(start));
        ArgumentOutOfRangeException.ThrowIfLessThan(end, start, nameof(end));

        this.Start = start;
        this.End = end;
        this.Length = end - start + TNumber.One;
    }

    public TNumber Start { get; init; }

    public TNumber End { get; init; }

    public TNumber Length { get; init; }

    public bool Contains(TNumber value)
        => value >= this.Start && value <= this.End;

    public bool Contains(SimpleRange<TNumber> other)
        => this.Start <= other.Start && this.End >= other.End;

    public bool Intersects(SimpleRange<TNumber> other)
        => this.Start <= other.End && other.Start <= this.End;

    public SimpleRange<TNumber> Intersection(SimpleRange<TNumber> other)
    {
        if (!this.Intersects(other))
        {
            throw new InvalidOperationException("Ranges do not intersect.");
        }

        var newStart = TNumber.Max(this.Start, other.Start);
        var newEnd = TNumber.Min(this.End, other.End);
        return new SimpleRange<TNumber>(newStart, newEnd);
    }

    public SimpleRange<TNumber> Combine(SimpleRange<TNumber> other)
    {
        if (this.Equals(other) || this.Contains(other))
        {
            return this;
        }

        if (other.Contains(this))
        {
            return other;
        }

        var newStart = TNumber.Min(this.Start, other.Start);
        var newEnd = TNumber.Max(this.End, other.End);
        return new SimpleRange<TNumber>(newStart, newEnd);
    }

    public RangeEnumerator<TNumber> GetEnumerator()
        => new(this.Start, this.End);

    public void Deconstruct(out TNumber start, out TNumber end)
    {
        start = this.Start;
        end = this.End;
    }
}
