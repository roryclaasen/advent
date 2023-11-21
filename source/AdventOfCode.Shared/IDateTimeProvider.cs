namespace AdventOfCode.Shared;

using System;

public interface IDateTimeProvider
{
    DateTime Now { get; }

    DateTime UTCNow { get; }

    DateTime AoCNow { get; }
}
