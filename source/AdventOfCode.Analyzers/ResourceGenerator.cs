// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using System;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ResourceGenerator : IIncrementalGenerator
{
    private static readonly char[] PathSeparators = ['/', '\\'];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var resourcePipeline = context.AdditionalTextsProvider
            .Select(static (file, ct) => TryParseResource(file, ct))
            .Where(static info => info is not null)
            .Select(static (info, _) => info!.Value);

        context.RegisterSourceOutput(resourcePipeline, static (context, model) =>
        {
            context.AddSource($"{model.ClassName}-{model.FileName}.g.cs", SourceText.From(
                $$"""
                namespace {{model.Namespace}}
                {
                    public partial class {{model.ClassName}} : global::{{model.Interface}}
                    {
                        {{GeneratorConstants.CompilerAttributes}}
                        public string {{model.FileName}} => {{SymbolDisplay.FormatLiteral(model.Contents ?? string.Empty, quote: true)}};
                    }
                }
                """,
                Encoding.UTF8));
        });
    }

    private static ResourceInfo? TryParseResource(AdditionalText file, CancellationToken ct)
    {
        var path = file.Path;
        if (!path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var fileName = Path.GetFileNameWithoutExtension(path);
        if (fileName != "Input" && fileName != "Expected1" && fileName != "Expected2")
        {
            return null;
        }

        var parts = path.Split(PathSeparators);
        if (parts.Length < 3 || !parts[parts.Length - 3].StartsWith("AdventOfCode.", StringComparison.Ordinal))
        {
            return null;
        }

        var inputNamespace = parts[parts.Length - 3];
        var className = $"{parts[parts.Length - 2]}Solution";
        var contents = file.GetText(ct)?.ToString();
        return new ResourceInfo(inputNamespace, className, fileName, contents);
    }
}
