// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;

internal interface IHasConfiguration : INukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public Configuration Configuration
        => this.TryGetValue(() => this.Configuration)
        ?? (this.IsLocalBuild ? Configuration.Debug : Configuration.Release);
}
