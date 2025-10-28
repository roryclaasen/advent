// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Commands;

using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

internal abstract class BaseCommand : Command
{
    public BaseCommand(string name, string? description = null)
        : base(name, description)
    {
        this.SetAction(this.ExecuteAsync);
    }

    protected abstract Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken);
}
