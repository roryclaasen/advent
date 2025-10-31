// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

internal interface IPublish : ICompile, IHasArtifacts
{
    [Parameter("Output folder for binaries")]
    public AbsolutePath? PublishDirectory => default;

    private AbsolutePath PublishDirectoryOrDefault => this.PublishDirectory ?? this.ArtifactsDirectoryOrDefault / "bin";

    public Target Publish => _ => _
        .Requires(() => this.Solution)
        .Requires(() => this.Configuration)
        .DependsOn(this.Compile)
        .Produces(this.PublishDirectoryOrDefault)
        .Executes(this.RunPublish);

    private void RunPublish()
    {
        this.PublishDirectoryOrDefault.DeleteDirectory();

        var settings = new DotNetPublishSettings()
            .EnableNoRestore()
            .EnableNoBuild()
            .SetProperty("PublishDir", this.PublishDirectoryOrDefault)
            .SetConfiguration(this.Configuration)
            .SetProject(this.Solution);

        DotNetTasks.DotNetPublish(settings);
    }
}
