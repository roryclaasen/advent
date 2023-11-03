namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;

[Problem(2015, 1, "Not Quite Lisp")]
public class Day1Solution : ISolver
{
    public object? PartOne(string input)
    {
        var floor = 0;
        foreach (var c in input)
        {
            if (c == '(')
            {
                floor++;
            }
            else if (c == ')')
            {
                floor--;
            }
        }

        return floor;
    }

    public object? PartTwo(string input)
    {
        var floor = 0;
        var position = 0;
        foreach (var c in input)
        {
            position++;

            if (c == '(')
            {
                floor++;
            }
            else if (c == ')')
            {
                floor--;
            }

            if (floor < 0)
            {
                break;
            }
        }

        return position;
    }
}
