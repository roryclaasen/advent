// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;
using Nuke.Common.IO;

internal interface IHasArtifacts : INukeBuild
{
    AbsolutePath? ArtifactsDirectory { get; }

    AbsolutePath ArtifactsDirectoryOrDefault => this.ArtifactsDirectory ?? this.RootDirectory / "artifacts";
}
