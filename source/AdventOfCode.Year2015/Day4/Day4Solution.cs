namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System.Security.Cryptography;
using System.Text;

[Problem(2015, 4, "The Ideal Stocking Stuffer")]
public class Day4Solution : ISolver
{
    public object? PartOne(string input)
    {
        int? number = null;
        var inputNumber = 0;
        do
        {
            var secret = $"{input}{inputNumber}";

            var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(secret));
            var hash = this.MakeHashReadable(hashBytes);
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
            var hash = this.MakeHashReadable(hashBytes);
            if (hash.StartsWith("000000"))
            {
                number = inputNumber;
            }

            inputNumber++;
        }
        while (number is null);

        return number;
    }

    private string MakeHashReadable(byte[] input)
    {
        var sOutput = new StringBuilder(input.Length);
        for (var i = 0; i < input.Length; i++)
        {
            sOutput.Append(input[i].ToString("X2"));
        }

        return sOutput.ToString();
    }
}
