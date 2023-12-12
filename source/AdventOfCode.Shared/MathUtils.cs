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
}
