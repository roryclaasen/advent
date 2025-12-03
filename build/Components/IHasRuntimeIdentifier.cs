// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using Nuke.Common;

internal interface IHasRuntimeIdentifier : INukeBuild
{
    [Parameter]
    public string RID {
        get {
            var rid = this.TryGetValue(() => this.RID);
            return rid switch
            {
                not null => rid,
                _ when OperatingSystem.IsWindows() => "win-x64",
                _ when OperatingSystem.IsLinux() => "linux-x64",
                _ when OperatingSystem.IsMacOS() => "osx-x64",
                _ => string.Empty
            };
        }
    }
}
