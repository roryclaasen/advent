namespace AdventOfCode.Shared;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

public static partial class IIEnumerableExtensions
{
    public static int Product(this IEnumerable<int> x) => Product<int, int>(x);

    public static long Product(this IEnumerable<long> x) => Product<long, long>(x);

    public static float Product(this IEnumerable<float> x) => Product<float, float>(x);

    public static double Product(this IEnumerable<double> x) => Product<double, double>(x);

    public static decimal Product(this IEnumerable<decimal> x) => Product<decimal, decimal>(x);

    public static TResult Product<TSource, TResult>(this IEnumerable<TSource> x)
        where TSource : struct, INumber<TSource>
        where TResult : struct, INumber<TResult>
    {
        TResult result = TResult.One;
        foreach (var item in x)
        {
            checked
            {
                result *= TResult.CreateChecked(item);
            }
        }

        return result;
    }

    public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> source)
    {
        var materialized = source.ToList();
        for (var i = 0; i < materialized.Count; i++)
        {
            yield return (materialized[i], i);
        }
    }
}
