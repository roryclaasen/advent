// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;

public class Build : NukeBuild, IClean, IRestore, ICompile, ITest, IPublish
{
    public static int Main() => Execute<Build>(x => ((ICompile)x).Compile);
}
