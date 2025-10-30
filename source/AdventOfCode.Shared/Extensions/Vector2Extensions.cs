// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Extensions;

using System.Collections.Generic;
using System.Numerics;

public static class Vector2Extensions
{
    public static IEnumerable<Vector2> EightNeighbors(this Vector2 vector)
    {
        foreach (var neighbor in vector.FourNeighbors())
        {
            yield return neighbor;
        }

        foreach (var neighbor in vector.CornerNeighbors())
        {
            yield return neighbor;
        }
    }

    public static IEnumerable<Vector2> FourNeighbors(this Vector2 vector)
    {
        yield return vector + Vector2.UnitX; // right
        yield return vector - Vector2.UnitX; // left
        yield return vector + Vector2.UnitY; // down
        yield return vector - Vector2.UnitY; // up
    }

    public static IEnumerable<Vector2> CornerNeighbors(this Vector2 vector)
    {
        yield return vector + Vector2.One; // down right
        yield return vector - Vector2.One; // up left
        yield return vector + new Vector2(1, -1); // up right
        yield return vector - new Vector2(1, -1); // down left
    }
}
