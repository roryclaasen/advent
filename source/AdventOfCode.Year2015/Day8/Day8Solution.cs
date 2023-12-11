namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

[Problem(2015, 8, "Matchsticks")]
public class Day8Solution : ISolver
{
    public object? PartOne(string input)
    {
        var entries = input.Lines().Select(l => new Entry(l, Decode(l)));
        var rawLength = entries.Sum(e => e.Raw.Length);
        var parsedLength = entries.Sum(e => e.Parsed.Length);

        return rawLength - parsedLength;
    }

    public object? PartTwo(string input)
    {
        var entries = input.Lines().Select(l => new Entry(l, Encode(l)));
        var rawLength = entries.Sum(e => e.Raw.Length);
        var parsedLength = entries.Sum(e => e.Parsed.Length);

        return parsedLength - rawLength;
    }

    private static string Decode(string input)
    {
        if (!input.StartsWith('"') || !input.EndsWith('"'))
        {
            throw new Exception("Invalid input string");
        }

        var trimed = input[1..^1];
        var sb = new StringBuilder();
        for (var i = 0; i < trimed.Length;)
        {
            var currentHead = trimed[i];
            if (currentHead == '\\')
            {
                if (i == trimed.Length - 1)
                {
                    throw new Exception("Invalid escape sequence (Out Of Bounds)");
                }

                var next = trimed[i + 1];
                if (next == '\\' || next == '"')
                {
                    sb.Append(next);
                    i += 2;
                }
                else if (next == 'x')
                {
                    var hex = trimed[(i + 2)..(i + 4)];
                    var decValue = uint.Parse(hex, NumberStyles.HexNumber);
                    var character = Convert.ToChar(decValue);
                    sb.Append(character);
                    i += 4;
                }
                else
                {
                    throw new Exception("Invalid escape sequence");
                }
            }
            else
            {
                sb.Append(currentHead);
                i++;
            }
        }
        return sb.ToString();
    }

    private static string Encode(string input)
    {
        var sb = new StringBuilder();
        sb.Append('"');
        foreach (var character in input)
        {
            var encodedChar = character switch
            {
                '"' => @"\""",
                '\\' => @"\\",
                _ => character.ToString()
            };

            sb.Append(encodedChar);
        }
        sb.Append('"');
        return sb.ToString();
    }

    private record Entry(string Raw, string Parsed);
}
