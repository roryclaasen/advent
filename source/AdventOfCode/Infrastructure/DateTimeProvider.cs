namespace AdventOfCode.Infrastructure;

using AdventOfCode.Shared;
using System;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UTCNow => DateTime.UtcNow;

    public DateTime AoCNow => this.UTCNow.AddHours(-5);
}
