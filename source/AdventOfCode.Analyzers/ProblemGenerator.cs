namespace AdventOfCode.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Linq;
using System.Text;

[Generator]
public class ProblemGenerator : IIncrementalGenerator
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

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            "AdventOfCode.Shared.ProblemAttribute",
            predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax,
            transform: static (context, ct) =>
            {
                var containingClass = context.TargetSymbol;
                var attribute = context.Attributes[0];
                return new ProblemInfo(
                    containingClass.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)),
                    containingClass.Name,
                    int.Parse(attribute.ConstructorArguments[0].Value?.ToString()),
                    int.Parse(attribute.ConstructorArguments[1].Value?.ToString()),
                    attribute.ConstructorArguments.Length == 3 ? attribute.ConstructorArguments[2].Value?.ToString() : null);
            });

        context.RegisterSourceOutput(pipeline, static (context, model) => context.AddSource($"{model.ClassName}.g.cs", SourceText.From($$"""
            {{FileHeaderComment}}

            namespace {{model.Namespace}}
            {
                public partial class {{model.ClassName}} : AdventOfCode.Shared.IProblemDetails
                {
                    public int Year => {{model.Year}};
                    public int Day => {{model.Day}};
                    public string Name => {{(string.IsNullOrWhiteSpace(model.Name) ? "string.Empty" : $"\"{model.Name}\"")}};
                }
            }
            """, Encoding.UTF8)));

        var resourcePipeline = context.AdditionalTextsProvider
            .Where(static f => f.Path.EndsWith(".txt"))
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

        context.RegisterSourceOutput(resourcePipeline, static (context, model) =>
        {
            var hasNewlines = model.Contents?.Lines.Count > 1;
            context.AddSource($"{model.ClassName}-{model.FileName}.g.cs", SourceText.From($$"""
                {{FileHeaderComment}}

                namespace {{model.Namespace}}
                {
                    public partial class {{model.ClassName}}
                    {
                        public const string {{model.FileName}} = @"{{model.Contents}}";
                    }
                }
                """, Encoding.UTF8));
        });
    }
}

public readonly record struct ProblemInfo
{
    public readonly string Namespace;
    public readonly string ClassName;
    public readonly int Year;
    public readonly int Day;
    public readonly string? Name;

    public ProblemInfo(string Namespace, string className, int year, int day, string? name)
    {
        this.Namespace = Namespace;
        this.ClassName = className;
        this.Year = year;
        this.Day = day;
        this.Name = name;
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
}
