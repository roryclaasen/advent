namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2015, 17, "No Such Thing as Too Much")]
public partial class Day17Solution : IProblemSolver
{
    public int MaxLiters { get; set; } = 150;

    public object? PartOne(string input)
    {
        var index = 0;
        var containers = input.Lines().Select(int.Parse).ToDictionary(_ => index++, i => i);

        var combinations = FindAllCombinations(containers, this.MaxLiters);

        return combinations.Count;
    }

    public object? PartTwo(string input)
    {
        var index = 0;
        var containers = input.Lines().Select(int.Parse).ToDictionary(_ => index++, i => i);

        var combinations = FindAllCombinations(containers, this.MaxLiters);

        return combinations.MinBy(c => c.Length)?.Length;
    }

    private static List<int[]> FindAllCombinations(Dictionary<int, int> numbers, int target)
    {
        var combinations = new List<int[]>();

        foreach (var number in numbers)
        {
            var remaining = target - number.Value;

            if (remaining == 0)
            {
                combinations.Add([number.Value]);
            }
            else if (remaining > 0)
            {
                var remainingNumbers = numbers.Where(n => n.Key > number.Key).ToDictionary(n => n.Key, n => n.Value);
                var remainingCombinations = FindAllCombinations(remainingNumbers, remaining);

                foreach (var combination in remainingCombinations)
                {
                    combinations.Add([number.Value, .. combination]);
                }
            }
        }

        return combinations;
    }
}
