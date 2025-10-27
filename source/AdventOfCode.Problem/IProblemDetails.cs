// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Problem;

public interface IProblemDetails
{
    public int Year { get; }

    public int Day { get; }

    public string Name { get; }
}
