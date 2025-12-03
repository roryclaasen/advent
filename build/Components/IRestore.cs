// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.Tools.DotNet;

internal interface IRestore : IHasSolution
{
    public Target Restore => _ => _
        .Requires(() => this.Solution)
        .Executes(this.RunRestore);

    private void RunRestore()
    {
        var settings = new DotNetRestoreSettings()
            .SetProjectFile(this.Solution);

        DotNetTasks.DotNetRestore(settings);
    }
}
