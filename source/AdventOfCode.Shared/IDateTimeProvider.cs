// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;

public interface IDateTimeProvider
{
    DateTime Now { get; }

    DateTime UTCNow { get; }

    DateTime AoCNow { get; }
}
