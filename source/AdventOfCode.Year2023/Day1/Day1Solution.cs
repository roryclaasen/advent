namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System.Linq;

[Problem(2023, 1, "Trebuchet?!")]
public class Day1Solution : ISolver
{
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
        foreach (var line in input.Lines().Select(RewriteLine))
        {
            var digits = line.Where(char.IsDigit);
            var number = $"{digits.First()}{digits.Last()}";
            sum += int.Parse(number);
        }

        return sum;
    }

    string RewriteLine(string line) => line
        .Replace("eightwo", "82")
        .Replace("oneight", "18")
        .Replace("threeight", "38")
        .Replace("fiveight", "58")
        .Replace("sevenine", "79")
        .Replace("twone", "21")
        .Replace("one", "1")
        .Replace("two", "2")
        .Replace("three", "3")
        .Replace("four", "4")
        .Replace("five", "5")
        .Replace("six", "6")
        .Replace("seven", "7")
        .Replace("eight", "8")
        .Replace("nine", "9");
}
