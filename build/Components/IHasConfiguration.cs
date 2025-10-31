// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Nuke.Common;

internal interface IHasConfiguration : INukeBuild
{
    Configuration Configuration { get; }
}
