namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;

public static class DivisorExtensions
{
    public static IEnumerable<uint> GetFactors(this uint number)
    {
        var factors = new HashSet<uint>();
        var max = (uint)Math.Sqrt(number);

        for (uint factor = 1; factor <= max; ++factor)
        {
            if (number % factor == 0)
            {
                factors.Add(factor);
                var otherFactor = number / factor;
                if (factor != otherFactor)
                {
                    factors.Add(otherFactor);
                }
            }
        }

        return factors;
    }

    public static IEnumerable<int> GetFactors(this int number)
    {
        var factors = new HashSet<int>();
        var max = (int)Math.Sqrt(number);

        for (int factor = 1; factor <= max; ++factor)
        {
            if (number % factor == 0)
            {
                factors.Add(factor);
                var otherFactor = number / factor;
                if (factor != otherFactor)
                {
                    factors.Add(otherFactor);
                }
            }
        }

        return factors;
    }
}
