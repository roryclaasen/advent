namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Linq;

[Problem(2015, 20, "Infinite Elves and Infinite Houses")]
public class Day20Solution : ISolver
{
    public object? PartOne(string input)
    {
        var target = int.Parse(input) / 10;

        for (int i = 0; i < target; i++)
        {
            var factors = i.GetFactors();
            if (factors.Sum() >= target)
            {
                return i;
            }
        }

        return null;
    }

    public object? PartTwo(string input)
    {
        var target = int.Parse(input) / 11;

        for (int i = 0; i < target; i++)
        {
            var factors = i.GetFactors().Where(d => i / d <= 50);
            if (factors.Sum() >= target)
            {
                return i;
            }
        }

        return null;
    }
}
