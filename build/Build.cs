// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(CI), nameof(CD) })]
public class Build : NukeBuild, IClean, IRestore, ICompile, ITest, IPublish
{
    public Target CI => _ => _
        .DependsOn(((IRestore)this).Restore)
        .DependsOn(((ICompile)this).Compile)
        .DependsOn(((ITest)this).Test);

    public Target CD => _ => _
        .DependsOn(((IPublish)this).Publish);

    public static int Main() => Execute<Build>(x => ((ICompile)x).Compile);
}
