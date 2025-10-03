namespace AdventOfCode.Shared;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static partial class IIEnumerableExtensions
{
    public static TNumber Product<TNumber>(this IEnumerable<TNumber> source)
        where TNumber : struct, INumber<TNumber>
        => Product<TNumber, TNumber>(source);

    public static TResult Product<TSource, TResult>(this IEnumerable<TSource> source)
        where TSource : struct, INumber<TSource>
        where TResult : struct, INumber<TResult>
    {
        TResult result = TResult.One;
        foreach (var item in source)
        {
            checked
            {
                result *= TResult.CreateChecked(item);
            }
        }

        return result;
    }

    public static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> source)
        => source.Select((item, index) => (item, index));
}
