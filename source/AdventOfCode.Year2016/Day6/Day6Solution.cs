namespace AdventOfCode.Year2016;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2016, 6, "Signals and Noise")]
public class Day6Solution : ISolver
{
    public int CharacterCount { get; set; } = 8;

    public object? PartOne(string input)
    {
        var charDict = new Dictionary<char, int>[this.CharacterCount];
        foreach (var line in input.Lines())
        {
            foreach (var (c, i) in line.WithIndex())
            {
                if (charDict[i] is null)
                {
                    charDict[i] = [];
                }

                if (charDict[i].TryGetValue(c, out int value))
                {
                    charDict[i][c] = ++value;
                }
                else
                {
                    charDict[i].Add(c, 1);
                }
            }
        }

        var message = new char[this.CharacterCount];
        for(var i = 0; i < this.CharacterCount; i++)
        { 
            message[i] = charDict[i].MaxBy(x => x.Value).Key;
        }

        return new string(message);
    }

    public object? PartTwo(string input)
    {
        var charDict = new Dictionary<char, int>[this.CharacterCount];
        foreach (var line in input.Lines())
        {
            foreach (var (c, i) in line.WithIndex())
            {
                if (charDict[i] is null)
                {
                    charDict[i] = [];
                }

                if (charDict[i].TryGetValue(c, out int value))
                {
                    charDict[i][c] = ++value;
                }
                else
                {
                    charDict[i].Add(c, 1);
                }
            }
        }

        var message = new char[this.CharacterCount];
        for (var i = 0; i < this.CharacterCount; i++)
        {
            message[i] = charDict[i].MinBy(x => x.Value).Key;
        }

        return new string(message);
    }
}
