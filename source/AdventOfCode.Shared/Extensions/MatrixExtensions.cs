// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

public static class MatrixExtensions
{
    public static T[,] ToMatrix<T>(this T[][] source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var matrix = new T[source.Length, source[0].Length];

        for (var y = 0; y < source.Length; y++)
        {
            if (source[y].Length != source[0].Length)
            {
                throw new ArgumentException("All rows must have the same length.", nameof(source));
            }

            for (var x = 0; x < source[y].Length; x++)
            {
                matrix[y, x] = source[y][x];
            }
        }

        return matrix;
    }

    public static int[,] ToMatrixInt(this IEnumerable<string> source)
        => ToMatrix(source, c => c - '0');

    public static bool[,] ToMatrixBool(this IEnumerable<string> source, char trueChar = '#')
        => ToMatrix(source, c => c == trueChar);

    public static char[,] ToMatrix(this IEnumerable<string> source)
        => ToMatrix(source, c => c);

    public static T[,] ToMatrix<T>(this IEnumerable<string> source, Func<char, T> charTransform)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var sourceArray = source.ToArray();

        var height = sourceArray.Length;
        var width = sourceArray[0].Length;

        var matrix = new T[height, width];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                matrix[y, x] = charTransform(sourceArray[y][x]);
            }
        }

        return matrix;
    }

    public static T[] GetRow<T>(this T[,] source, int rowIndex)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return [.. Enumerable.Range(0, source.GetLength(1)).Select(x => source[rowIndex, x])];
    }

    public static T[] GetColumn<T>(this T[,] source, int columnIndex)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return [.. Enumerable.Range(0, source.GetLength(0)).Select(x => source[x, columnIndex])];
    }

    public static T[,] Rotate<T>(this T[,] source, bool clockwise = true)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var width = source.GetLength(0);
        var height = source.GetLength(1);

        var result = new T[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                result[y, x] = clockwise
                    ? source[width - x - 1, y]
                    : source[x, height - y - 1];
            }
        }

        return result;
    }

    public static T[,] Flip<T>(this T[,] source, bool horizontal = true)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var width = source.GetLength(0);
        var height = source.GetLength(1);

        var result = new T[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                result[y, x] = horizontal
                    ? source[y, width - x - 1]
                    : source[height - y - 1, x];
            }
        }

        return result;
    }

    public static string Render(this bool[,] source, char trueChar = '#', char falseChar = ' ')
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var sb = new StringBuilder();
        var lastY = source.GetLength(0) - 1;
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++)
            {
                sb.Append(source[y, x] ? trueChar : falseChar);
            }

            if (y < lastY)
            {
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    public static int Count<T>(this T[,] source, Func<T, bool>? predict = null)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var count = 0;
        foreach (var item in source)
        {
            if (predict is null || predict(item))
            {
                count++;
            }
        }

        return count;
    }

    public static int Count<T>(this T[,] source, Func<T, Vector2, bool> predict)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(predict, nameof(predict));

        var count = 0;
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++)
            {
                if (predict(source[y, x], new Vector2(x, y)))
                {
                    count++;
                }
            }
        }

        return count;
    }
}
