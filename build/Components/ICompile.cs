// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.Tools.DotNet;

internal interface ICompile : IRestore, IHasConfiguration
{
    public Target Compile => _ => _
        .Requires(() => this.Solution)
        .Requires(() => this.Configuration)
        .DependsOn(this.Restore)
        .Executes(this.RunCompile);

    private void RunCompile()
    {
        var settings = new DotNetBuildSettings()
            .EnableNoRestore()
            .SetConfiguration(this.Configuration)
            .SetProjectFile(this.Solution);

        DotNetTasks.DotNetBuild(settings);
    }
}
