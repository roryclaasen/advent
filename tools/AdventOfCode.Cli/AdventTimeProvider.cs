// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli;

using System;

internal sealed class AdventTimeProvider : TimeProvider
{
    public static AdventTimeProvider Instance { get; } = new AdventTimeProvider();

    public override TimeZoneInfo LocalTimeZone { get; } = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
}
