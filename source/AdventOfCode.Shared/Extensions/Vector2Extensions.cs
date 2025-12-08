// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System.Numerics;

public static class Vector2Extensions
{
    public static void Deconstruct(this Vector2 vector, out float x, out float y)
    {
        x = vector.X;
        y = vector.Y;
    }

    public static bool WithinBounds(this Vector2 vector, Vector2 min, Vector2 max)
        => vector.WithinBounds(min.X, min.Y, max.X, max.Y);

    public static bool WithinBounds(this Vector2 vector, Vector2 start, float width, float height)
        => vector.WithinBounds(start.X, start.Y, start.X + width, start.Y + height);

    public static bool WithinBounds(this Vector2 vector, float x1, float y1, float x2, float y2)
        => vector.X >= x1 && vector.X < x2 && vector.Y >= y1 && vector.Y < y2;

    public static bool WithinBounds(this Vector2 vector, float width, float height)
        => vector.WithinBounds(0, 0, width, height);
}
