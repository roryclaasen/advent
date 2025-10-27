// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Numerics;

public static class DigitExtensions
{
    public static int CountDigits<TNumber>(this TNumber self) where TNumber : struct, INumber<TNumber>
        => (int)(Math.Log10(double.CreateChecked(self)) + 1);

    public static (TNumber Left, TNumber Right) SplitDigits<TNumber>(this TNumber self, int length) where TNumber : struct, INumber<TNumber>
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        var digits = self.CountDigits();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, digits, nameof(length));

        var divisor = TNumber.One;
        for (var i = 0; i < length; i++)
        {
            divisor *= TNumber.CreateChecked(10);
        }

        var left = self / divisor;
        var right = self % divisor;
        return (left, right);
    }
}
