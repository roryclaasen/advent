
namespace AdventOfCode.Shared;

using System;
using System.Diagnostics;
using System.Numerics;

public static class DirectionExtensions
{
    public static Direction Rotate(this Direction self, int direction = 1)
    {
        if (direction == -1)
        {
            return self switch
            {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                _ => throw new UnreachableException(),
            };
        }
        else if (direction == 1)
        {
            return self switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new UnreachableException(),
            };
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(direction), "Direction must be -1 or 1");
        }
    }

    public static Vector2 ToVector(this Direction self) => self switch
    {
        Direction.Up => new Vector2(0, -1),
        Direction.Right => new Vector2(1, 0),
        Direction.Down => new Vector2(0, 1),
        Direction.Left => new Vector2(-1, 0),
        _ => throw new UnreachableException(),
    };
}
