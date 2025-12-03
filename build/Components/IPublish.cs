// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

internal interface IPublish : ICompile, IHasRuntimeIdentifier, IHasArtifacts
{
    [Parameter("Output folder for binaries")]
    public AbsolutePath PublishDirectory
        => this.TryGetValue(() => this.PublishDirectory)
        ?? (this.ArtifactsDirectory / "bin");

    public Target Publish => _ => _
        .Requires(() => this.Solution)
        .Requires(() => this.Configuration)
        .DependsOn(this.Compile)
        .Produces(this.PublishDirectory)
        .Executes(this.RunPublish);

    public Target PublishAOT => _ => _
        .Requires(() => this.Solution)
        .Requires(() => this.Configuration)
        .Requires(() => this.RID)
        .Produces(this.PublishDirectory)
        .Executes(this.RunPublishAOT);

    private void RunPublish()
    {
        this.PublishDirectory.DeleteDirectory();

        var settings = new DotNetPublishSettings()
            .EnableNoRestore()
            .EnableNoBuild()
            .SetProperty("PublishDir", this.PublishDirectory)
            .SetConfiguration(this.Configuration)
            .SetProject(this.Solution);

        DotNetTasks.DotNetPublish(settings);
    }

    private void RunPublishAOT()
    {
        this.PublishDirectory.DeleteDirectory();

        var settings = new DotNetPublishSettings()
            .SetProperty("PublishDir", this.PublishDirectory)
            .SetConfiguration(this.Configuration)
            .SetProject(this.Solution)
            .SetRuntime(this.RID);

        DotNetTasks.DotNetPublish(settings);
    }
}
