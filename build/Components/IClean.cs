// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.Tools.DotNet;

internal interface IClean : IRestore, IHasConfiguration
{
    public Target Clean => _ => _
        .Before(this.Restore)
        .Requires(() => this.Solution)
        .Requires(() => this.Configuration)
        .Executes(this.RunClean);

    private void RunClean()
    {
        var settings = new DotNetCleanSettings()
            .SetConfiguration(this.Configuration)
            .SetProject(this.Solution);

        DotNetTasks.DotNetClean(settings);
    }
}
