// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Problem;

using System;

public class ProgressEventArgs(int current, int total) : EventArgs
{
    public int Current { get; } = current;

    public int Total { get; } = total;
}
