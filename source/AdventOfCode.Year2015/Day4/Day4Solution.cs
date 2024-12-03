namespace AdventOfCode.Year2015;

using AdventOfCode.Problem;
using System.Security.Cryptography;
using System.Text;

[Problem(2015, 4, "The Ideal Stocking Stuffer")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        int? number = null;
        var inputNumber = 0;
        do
        {
            var secret = $"{input}{inputNumber}";

            var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(secret));
            var hash = MakeHashReadable(hashBytes);
            if (hash.StartsWith("00000"))
            {
                number = inputNumber;
            }

            inputNumber++;
        }
        while (number is null);

        return number;
    }

    public object? PartTwo(string input)
    {
        int? number = null;
        var inputNumber = 0;
        do
        {
            var secret = $"{input}{inputNumber}";

            var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(secret));
            var hash = MakeHashReadable(hashBytes);
            if (hash.StartsWith("000000"))
            {
                number = inputNumber;
            }

            inputNumber++;
        }
        while (number is null);

        return number;
    }

    private static string MakeHashReadable(byte[] input)
    {
        var sOutput = new StringBuilder(input.Length);
        for (var i = 0; i < input.Length; i++)
        {
            sOutput.Append(input[i].ToString("X2"));
        }

        return sOutput.ToString();
    }
}
