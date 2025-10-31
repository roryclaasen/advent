// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.ProjectModel;

internal interface IHasSolution : INukeBuild
{
    [Solution(SuppressBuildProjectCheck = true)]
    public Solution? Solution => this.TryGetValue(() => this.Solution);
}
