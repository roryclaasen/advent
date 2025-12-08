// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Problem;

using System;

public interface IHasProgress
{
    /// <summary>
    /// Event that is raised when progress is made.
    /// </summary>
    event EventHandler<ProgressEventArgs>? ProgressChanged;
}
