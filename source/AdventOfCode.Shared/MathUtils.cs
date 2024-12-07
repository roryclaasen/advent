namespace AdventOfCode.Shared;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class MathUtils
{
    /// <summary>
    /// Returns the greatest common divisor of two numbers.
    /// </summary>
    /// <typeparam name="TNumber">The number type to use.</typeparam>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The greatest common divisor.</returns>
    public static TNumber Gcd<TNumber>(TNumber a, TNumber b) where TNumber : struct, INumber<TNumber>
    {
        if (b == TNumber.Zero)
        {
            return a;
        }

        return Gcd(b, a % b);
    }

    /// <summary>
    /// Returns the least common multiple of two numbers.
    /// </summary>
    /// <typeparam name="TNumber">The number type to use.</typeparam>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The least common multiple.</returns>
    public static TNumber Lcm<TNumber>(TNumber a, TNumber b) where TNumber : struct, INumber<TNumber>
        => checked(a * b) / Gcd(a, b);

    /// <summary>
    /// Returns the least common multiple of a collection of numbers.
    /// </summary>
    /// <typeparam name="TNumber">The number type to use.</typeparam>
    /// <param name="numbers">The collection of numbers.</param>
    /// <returns>The least common multiple.</returns>
    public static TNumber Lcm<TNumber>(IEnumerable<TNumber> numbers) where TNumber : struct, INumber<TNumber>
        => numbers.Aggregate(Lcm);

    /// <summary>
    /// Fast concatenation of two uints.
    /// <see href="https://stackoverflow.com/a/26853517/4498839"/>
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The concatenated number.</returns>
    public static ulong FastConcat(ulong a, ulong b)
    {
        checked
        {
            return b switch
            {
                (< 10U) => 10UL * a + b,
                (< 100U) => 100UL * a + b,
                (< 1000U) => 1000UL * a + b,
                (< 10000U) => 10000UL * a + b,
                (< 100000U) => 100000UL * a + b,
                (< 1000000U) => 1000000UL * a + b,
                (< 10000000U) => 10000000UL * a + b,
                (< 100000000U) => 100000000UL * a + b,
                _ => 1000000000UL * a + b
            };
        }
    }
}
