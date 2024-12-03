namespace AdventOfCode.Year2017;

using AdventOfCode.Shared;

[Problem(2017, 1, "Inverse Captcha")]
public partial class Day1Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var sum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var next = (i + 1) % input.Length;
            if (input[i] == input[next])
            {
                sum += int.Parse(input[i].ToString());
            }
        }

        return sum;
    }

    public object? PartTwo(string input)
    {
        var sum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var next = (i + input.Length / 2) % input.Length;
            if (input[i] == input[next])
            {
                sum += int.Parse(input[i].ToString());
            }
        }

        return sum;
    }
}
