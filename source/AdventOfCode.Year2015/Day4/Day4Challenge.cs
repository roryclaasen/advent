// ------------------------------------------------------------------------------
// <copyright file="Day4Challenge.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Year2015
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure;

    public class Day4Challenge : IChallenge
    {
        private readonly Lazy<MD5> mD5 = new(() => MD5.Create());

        public Task<string> SolvePart1(string input)
        {
            int? number = null;
            var inputNumber = 0;
            do
            {
                var secret = $"{input}{inputNumber}";

                var hashBytes = this.mD5.Value.ComputeHash(Encoding.ASCII.GetBytes(secret));
                var hash = this.MakeHashReadable(hashBytes);
                if (hash.StartsWith("00000"))
                {
                    number = inputNumber;
                }

                inputNumber++;
            }
            while (number == null);

            return this.Answer(number);
        }

        public Task<string> SolvePart2(string input)
        {
            int? number = null;
            var inputNumber = 0;
            do
            {
                var secret = $"{input}{inputNumber}";

                var hashBytes = this.mD5.Value.ComputeHash(Encoding.ASCII.GetBytes(secret));
                var hash = this.MakeHashReadable(hashBytes);
                if (hash.StartsWith("000000"))
                {
                    number = inputNumber;
                }

                inputNumber++;
            }
            while (number == null);

            return this.Answer(number);
        }

        private string MakeHashReadable(byte[] input)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(input.Length);
            for (i = 0; i < input.Length; i++)
            {
                sOutput.Append(input[i].ToString("X2"));
            }

            return sOutput.ToString();
        }
    }
}
