// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Extensions;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using AdventOfCode.Shared.Memory;
using CommunityToolkit.HighPerformance;
using LinkDotNet.StringBuilder;

public static class SpanExtensions
{
    public static Vector2 ToVector2(this ReadOnlySpan<char> span)
    {
        var cords = span.Split(',');

        var x = cords.MoveNext() ? cords.Current : throw new FormatException("Invalid Vector3 format.");
        var y = cords.MoveNext() ? cords.Current : throw new FormatException("Invalid Vector3 format.");

        return new Vector2(GetNumber<float>(span[x]), GetNumber<float>(span[y]));
    }

    public static Vector3 ToVector3(this ReadOnlySpan<char> span)
    {
        var cords = span.Split(',');

        var x = cords.MoveNext() ? cords.Current : throw new FormatException("Invalid Vector3 format.");
        var y = cords.MoveNext() ? cords.Current : throw new FormatException("Invalid Vector3 format.");
        var z = cords.MoveNext() ? cords.Current : throw new FormatException("Invalid Vector3 format.");

        return new Vector3(GetNumber<float>(span[x]), GetNumber<float>(span[y]), GetNumber<float>(span[z]));
    }

    public static ReadOnlySpan<char> GetWidthAndHeight(this ReadOnlySpan<char> span, out int width, out int height)
    {
        height = 0;
        width = 0;
        var sb = new ValueStringBuilder();
        foreach (var line in span.EnumerateLines())
        {
            sb.Append(line);
            height++;
            width = Math.Max(width, line.Length);
        }

        return sb.AsSpan();
    }

    public static ReadOnlySpan2D<char> AsSpan2D(this ReadOnlySpan<char> span)
        => span.GetWidthAndHeight(out var width, out var height).AsSpan2D(height, width);

    public static VerticalSplitEnumerator VerticalSplit(this string str, char separator)
        => new(str.AsSpan2D(), separator);

    public static VerticalSplitEnumerator VerticalSplit(this ReadOnlySpan<char> span, char separator)
    => new(span.AsSpan2D(), separator);

    public static VerticalSplitEnumerator VerticalSplit(this ReadOnlySpan2D<char> span, char separator)
        => new(span, separator);

    private static TNumber GetNumber<TNumber>(ReadOnlySpan<char> span) where TNumber : struct, INumber<TNumber>
        => TNumber.TryParse(span, CultureInfo.InvariantCulture, out var result)
        ? result
        : throw new FormatException("Invalid float format.");
}
