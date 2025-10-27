// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using AdventOfCode.Problem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

[Generator]
public class ProblemGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            typeof(ProblemAttribute).FullName,
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
            namespace {{model.Namespace}}
            {
                [System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
                public partial class {{model.ClassName}} : {{typeof(IProblemDetails).FullName}}
                {
                    [System.Runtime.CompilerServices.CompilerGenerated]
                    public int Year => {{model.Year}};

                    [System.Runtime.CompilerServices.CompilerGenerated]
                    public int Day => {{model.Day}};

                    [System.Runtime.CompilerServices.CompilerGenerated]
                    public string Name => {{(string.IsNullOrWhiteSpace(model.Name) ? "string.Empty" : $"\"{model.Name}\"")}};

                    [System.Runtime.CompilerServices.CompilerGenerated]
                    public override string ToString() => $"Year {this.Year} Day {this.Day}{(!string.IsNullOrWhiteSpace(this.Name) ? $" - {this.Name}" : string.Empty)}";
                }
            }
            """, Encoding.UTF8)));
    }

    private readonly record struct ProblemInfo
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
}
