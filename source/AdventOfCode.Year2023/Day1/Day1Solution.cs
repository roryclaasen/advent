namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System.Linq;

[Problem(2023, 1, "Trebuchet?!")]
public class Day1Solution : ISolver
{
    private readonly string[] NumberWords = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    public object? PartOne(string input)
    {
        var sum = 0;
        foreach (var line in input.Lines())
        {
            var digits = line.Where(char.IsDigit);
            var number = $"{digits.First()}{digits.Last()}";
            sum += int.Parse(number);
        }

        return sum;
    }

    public object? PartTwo(string input)
    {
        var sum = 0;
        foreach (var line in input.Lines())
        {
            var first = FindFirstNumber(line);
            var last = FindLastNumber(line);
            var number = $"{first}{last}";
            sum += int.Parse(number);
        }

        return sum;
    }

    int FindFirstNumber(string input)
    {
        int? value = null;
        int? currentIndex = null;
        foreach (var (number, numberIndex) in NumberWords.WithIndex())
        {
            var index = input.IndexOf(number);
            if (index == -1)
            {
                continue;
            }

            if (currentIndex is null || index < currentIndex)
            {
                currentIndex = index;
                value = numberIndex + 1;
            }
        }

        var digitIndex = input.IndexOfAny("123456789".ToCharArray());
        if (value is null || currentIndex is null || (digitIndex != -1 && currentIndex > digitIndex))
        {
            return int.Parse(input[digitIndex].ToString());
        }

        return value.Value;
    }

    int FindLastNumber(string input)
    {
        int? value = null;
        int? currentIndex = null;
        foreach (var (number, numberIndex) in NumberWords.WithIndex())
        {
            var index = input.LastIndexOf(number);
            if (index == -1)
            {
                continue;
            }

            if (currentIndex is null || index > currentIndex)
            {
                currentIndex = index;
                value = numberIndex + 1;
            }
        }

        var digitIndex = input.LastIndexOfAny("0123456789".ToCharArray());
        if (value is null || currentIndex is null || (digitIndex != -1 && currentIndex < digitIndex))
        {
            return int.Parse(input[digitIndex].ToString());
        }

        return value.Value;
    }
}
