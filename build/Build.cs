// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;

public class Build : NukeBuild, IClean, IRestore, ICompile, ITest, IPublish
{
    public Target CI => _ => _
        .DependsOn(((IRestore)this).Restore)
        .DependsOn(((ICompile)this).Compile)
        .DependsOn(((ITest)this).Test);

    public Target CD => _ => _
        .DependsOn(((IPublish)this).PublishAOT);

    public static int Main() => Execute<Build>(x => ((ICompile)x).Compile);
}
