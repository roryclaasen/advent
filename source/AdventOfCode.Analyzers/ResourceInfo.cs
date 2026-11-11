// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using System;
using AdventOfCode.Problem;

internal readonly record struct ResourceInfo
{
    public readonly string Namespace;
    public readonly string ClassName;
    public readonly string FileName;
    public readonly string? Contents;

    public ResourceInfo(string classNamespace, string className, string fileName, string? contents)
    {
        this.Namespace = classNamespace;
        this.ClassName = className;
        this.FileName = fileName;
        this.Contents = contents;
    }

    public string Interface => this.FileName switch
    {
        "Input" => typeof(IProblemInput).FullName,
        "Expected1" => typeof(IProblemExpectedResultPart1).FullName,
        "Expected2" => typeof(IProblemExpectedResultPart2).FullName,
        _ => throw new NotImplementedException()
    };
}
