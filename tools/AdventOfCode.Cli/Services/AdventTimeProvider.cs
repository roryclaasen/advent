// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Services;

using System;

internal class AdventTimeProvider : TimeProvider
{
    public static AdventTimeProvider Instance { get; } = new AdventTimeProvider();

    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.FindSystemTimeZoneById("EST");
}
