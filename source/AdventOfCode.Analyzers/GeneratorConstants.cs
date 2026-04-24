// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

internal static class GeneratorConstants
{
    public const string CompilerAttributes =
@$"#if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
        [global::System.Diagnostics.DebuggerNonUserCode]
        [global::System.CodeDom.Compiler.GeneratedCode(""{ThisAssembly.AssemblyName}"", ""{ThisAssembly.AssemblyFileVersion}"")]
        #endif
        #if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        #endif";
}
