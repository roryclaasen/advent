// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Infrastructure;

using AdventOfCode.Shared;
using System;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UTCNow => DateTime.UtcNow;

    public DateTime AoCNow => this.UTCNow.AddHours(-5);
}
