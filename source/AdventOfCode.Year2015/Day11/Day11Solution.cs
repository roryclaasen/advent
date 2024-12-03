namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;

[Problem(2015, 11, "Corporate Policy")]
public partial class Day11Solution : IProblemSolver
{
    public object? PartOne(string input) => this.FindNextPassword(input);

    public object? PartTwo(string input) => this.FindNextPassword(this.FindNextPassword(input));

    private string FindNextPassword(string current)
    {
        var newPassword = current;
        do
        {
            newPassword = IncrementPassword(newPassword);
        } while (!IsPasswordValid(newPassword));
        return newPassword;
    }

    private static string IncrementPassword(string password)
    {
        var newPassword = password.ToCharArray();
        for (var i = newPassword.Length - 1; i >= 0; i--)
        {
            if (newPassword[i] == 'z')
            {
                newPassword[i] = 'a';
            }
            else
            {
                newPassword[i]++;
                break;
            }
        }

        return new string(newPassword);
    }

    private static bool IsPasswordValid(string password)
    {
        if (password.Contains('i') || password.Contains('o') || password.Contains('l'))
        {
            return false;
        }

        var hasStraight = false;
        for (var i = 0; i < password.Length - 2; i++)
        {
            if (password[i] == password[i + 1] - 1 && password[i] == password[i + 2] - 2)
            {
                hasStraight = true;
                break;
            }
        }

        if (!hasStraight)
        {
            return false;
        }

        var pairCount = 0;
        for (var i = 0; i < password.Length - 1; i++)
        {
            if (password[i] == password[i + 1])
            {
                pairCount++;
                i++;
            }
        }

        return pairCount >= 2;
    }
}
