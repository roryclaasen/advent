namespace AdventOfCode.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[Generator]
public class ResourceGenerator : IIncrementalGenerator
{
    private const string FileHeaderComment = @"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        static bool IsResourceFileValid(string path)
        {
            HashSet<string> ValidFiles = ["Input", "Expected1", "Expected2"];
            var fileName = Path.GetFileNameWithoutExtension(path);
            return ValidFiles.Contains(fileName);
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

                var Namespace = pathParts.First();
                var className = $"{pathParts.Skip(1).First()}Solution";
                return new ResourceInfo(Namespace, className, fileName, f.GetText(ct));
            });

        context.RegisterSourceOutput(resourcePipeline, static (context, model) => context.AddSource($"{model.ClassName}-{model.FileName}.g.cs", SourceText.From($$"""
            {{FileHeaderComment}}

            namespace {{model.Namespace}}
            {
                public partial class {{model.ClassName}} : {{model.Interface}}
                {
                    public string {{model.FileName}} => @"{{model.Contents}}";
                }
            }
            """, Encoding.UTF8)));
    }
}

public readonly record struct ResourceInfo
{
    public readonly string Namespace;
    public readonly string ClassName;
    public readonly string FileName;
    public readonly SourceText? Contents;

    public ResourceInfo(string Namespace, string className, string fileName, SourceText? contents)
    {
        this.Namespace = Namespace;
        this.ClassName = className;
        this.FileName = fileName;
        this.Contents = contents;
    }

    public string Interface => this.FileName switch
    {
        "Input" => "AdventOfCode.Shared.IProblemInput",
        "Expected1" => "AdventOfCode.Shared.IProblemExpectedResultPart1",
        "Expected2" => "AdventOfCode.Shared.IProblemExpectedResultPart2",
        _ => throw new NotImplementedException()
    };
}
