namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Text;

[Problem(2015, 10, "Elves Look, Elves Say")]
public class Day10Solution : ISolver
{
    public object? PartOne(string input)
    {
        for (var i = 0; i < 40; i++)
        {
            input = LookAndSay(input);
        }

        return input.Length;
    }

    public object? PartTwo(string input)
    {
        for (var i = 0; i < 50; i++)
        {
            input = LookAndSay(input);
        }

        return input.Length;
    }

    public static string LookAndSay(string input)
    {
        var finalString = new StringBuilder();

        for (var i = 0; i < input.Length; i++)
        {
            var currentNumber = input[i];
            var count = 1;
            while (i + 1 < input.Length && input[i + 1] == currentNumber)
            {
                count++;
                i++;
            }

            finalString.Append(count);
            finalString.Append(currentNumber);
        }

        return finalString.ToString();
    }
}
