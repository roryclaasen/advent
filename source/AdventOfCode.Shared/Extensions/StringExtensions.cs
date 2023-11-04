namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static Vector2 ToVector2(this string str)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));

        var cords = str.Split(',').Select(int.Parse).ToArray();
        return new Vector2(cords[0], cords[1]);
    }

    /// <summary>
    /// Splits a string into n number of parts.
    /// <see href="https://stackoverflow.com/a/4133475/4498839"/>
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="partLength">The length of each part.</param>
    /// <param name="skip">The number of chars to skip before the next part.</param>
    /// <returns><see cref="IEnumerable{string}"/> of parts.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<string> SplitInParts(this string str, int partLength, int skip = 0)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        ArgumentNullException.ThrowIfNull(partLength, nameof(partLength));

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

        return string.Join("\n", str
            .SplitNewLine()
            .Select(line => Regex.Replace(line, @"^\s*" + Regex.Escape(margin), ""))
        );
    }

    public static string Indent(this string str, int length, bool firstLine = false)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        ArgumentNullException.ThrowIfNull(length, nameof(length));

        var indent = new string(' ', length);
        var res = string.Join(Environment.NewLine + new string(' ', length), str.SplitNewLine().Select(line => Regex.Replace(line, @"^\s*\|", "")));
        return firstLine ? indent + res : res;
    }

    /// <summary>
    /// Splits a string into substrings based on the <see cref="Environment.NewLine"/> delimiter.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from this instance that are delimited by <see cref="Environment.NewLine"/>.</returns>
    public static string[] SplitNewLine(this string str, StringSplitOptions options = StringSplitOptions.None)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        return str.Split(Environment.NewLine, options);
    }
}
