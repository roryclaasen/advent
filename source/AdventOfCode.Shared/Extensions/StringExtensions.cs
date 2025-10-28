// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

public static partial class StringExtensions
{
    public static Vector2 ToVector2(this string str)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));

        var cords = str.Split(',').Select(float.Parse).ToArray();
        return new Vector2(cords[0], cords[1]);
    }

    /// <summary>
    /// Splits a string into n number of parts.
    /// <see href="https://stackoverflow.com/a/4133475/4498839"/>
    /// </summary>
    /// <param name="str">The string to split.</param>
    /// <param name="partLength">The length of each part.</param>
    /// <param name="skip">The number of chars to skip before the next part.</param>
    /// <returns><see cref="IEnumerable{string}"/> of parts.</returns>
    public static IEnumerable<string> SplitInParts(this string str, int partLength, int skip = 0)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));

        if (partLength <= 0)
        {
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));
        }

        for (var i = 0; i < str.Length; i += partLength + skip)
        {
            yield return str.Substring(i, Math.Min(partLength, str.Length - i));
        }
    }

    public static string StripMargin(this string str, string margin = "|")
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        ArgumentNullException.ThrowIfNull(margin, nameof(margin));

        return string.Join(Environment.NewLine, str
            .Lines()
            .Select(line => Regex.Replace(line, @"^\s*" + Regex.Escape(margin), string.Empty)));
    }

    public static string Indent(this string str, int length, bool firstLine = false)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));

        var indent = new string(' ', length);
        var res = string.Join(Environment.NewLine + new string(' ', length), str.Lines().Select(line => MarginRegex().Replace(line, string.Empty)));
        return firstLine ? indent + res : res;
    }

    /// <summary>
    /// Splits a string into substrings based on the <see cref="Environment.NewLine"/> delimiter.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from this instance that are delimited by <see cref="Environment.NewLine"/>.</returns>
    public static string[] Lines(this string str, StringSplitOptions options = StringSplitOptions.None) => str.Lines(1, options);

    /// <summary>
    /// Splits a string into substrings based on the <see cref="Environment.NewLine"/> delimiter concatenate <see langword="count"/> times.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="count">The number of times to concatenate <see cref="Environment.NewLine"/>.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from this instance that are delimited by <see cref="Environment.NewLine"/>.</returns>
    public static string[] Lines(this string str, int count, StringSplitOptions options = StringSplitOptions.None)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        var separator = string.Concat(Enumerable.Repeat(Environment.NewLine, count));
        return str.Split(separator, options);
    }

    /// <summary>
    /// Splits a string into substrings based on a space delimiter concatenate <see langword="count"/> times.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="count">The number of times to concatenate a space.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from this instance that are delimited by a space.</returns>
    public static string[] Spaces(this string str, int count, StringSplitOptions options = StringSplitOptions.None)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        return str.Split(new string(' ', count), options);
    }

    /// <summary>
    /// Splits a string into substrings based on a space delimiter.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from this instance that are delimited by a space.</returns>
    public static string[] Spaces(this string str, StringSplitOptions options = StringSplitOptions.None) => str.Spaces(1, options);

    /// <summary>
    /// Reports the zero-based indexes of all the occurrences of a specified Unicode character or string within this instance.
    /// The method returns -1 if the character or string is not found in this instance.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="value">The string to seek.</param>
    /// <returns>An <see cref="IEnumerable{int}"/> containing all the zero-based index position of <see langword="value"/> from the start of the current instance if that string is found, or -1 if it is not.</returns>
    public static IEnumerable<int> AllIndexesOf(this string str, string value)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var minIndex = str.IndexOf(value, StringComparison.Ordinal);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(value, minIndex + value.Length, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// Reports the zero-based indexes of all the occurrences of a specified Unicode character or string within this instance.
    /// The method returns -1 if the character or string is not found in this instance.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="value">The string to seek.</param>
    /// <returns>An <see cref="IEnumerable{int}"/> containing all the zero-based index position of <see langword="value"/> from the start of the current instance if that string is found, or -1 if it is not.</returns>
    public static IEnumerable<int> AllIndexesOf(this string str, char value)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));

        var minIndex = str.IndexOf(value, StringComparison.Ordinal);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(value, minIndex + 1);
        }
    }

    [GeneratedRegex(@"^\s*\|")]
    private static partial Regex MarginRegex();
}
