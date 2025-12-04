// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ResourceGenerator : IIncrementalGenerator
{
    private const string CompilerAttributes = """
        #if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
                [global::System.Diagnostics.DebuggerNonUserCode]
                [global::System.CodeDom.Compiler.GeneratedCode("AdventOfCode.Generators", "1.0.0")]
                #endif
                #if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
                [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
                #endif
        """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        static bool IsResourceFileValid(string path)
        {
            HashSet<string> validFiles = ["Input", "Expected1", "Expected2"];
            var fileName = Path.GetFileNameWithoutExtension(path);
            return validFiles.Contains(fileName);
        }

        var resourcePipeline = context.AdditionalTextsProvider
            .Where(static f => f.Path.EndsWith(".txt") && IsResourceFileValid(f.Path))
            .Select(static (f, ct) =>
            {
                // TODO: A bit hacky but can't think of a better way to do this right now
                var fileName = Path.GetFileNameWithoutExtension(f.Path);

                var pathParts = f.Path
                    .Split(Path.DirectorySeparatorChar)
                    .SkipWhile(f => !f.StartsWith("AdventOfCode."));

                var inputNamespace = pathParts.First();
                var className = $"{pathParts.Skip(1).First()}Solution";
                return new ResourceInfo(inputNamespace, className, fileName, f.GetText(ct)?.ToString());
            });

        context.RegisterSourceOutput(resourcePipeline, static (context, model) =>
        {
            context.AddSource($"{model.ClassName}-{model.FileName}.g.cs", SourceText.From(
                $$""""
                namespace {{model.Namespace}}
                {
                    public partial class {{model.ClassName}} : global::{{model.Interface}}
                    {
                        {{CompilerAttributes}}
                        public string {{model.FileName}} =>
                            """
                            {{model.PadContents(12)}}
                            """;
                    }
                }
                """",
                Encoding.UTF8));
        });
    }
}
