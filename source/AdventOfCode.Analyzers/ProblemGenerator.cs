// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using System.Text;
using AdventOfCode.Problem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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

        context.RegisterSourceOutput(pipeline, static (context, model) =>
        {
            var compilerAttributes = $$"""
            #if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
                    [global::System.Diagnostics.DebuggerNonUserCode]
                    [global::System.CodeDom.Compiler.GeneratedCode("{{ThisAssembly.AssemblyName}}", "{{ThisAssembly.AssemblyVersion}}")]
                    #endif
                    #if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
                    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
                    #endif
            """;

            context.AddSource($"{model.ClassName}.g.cs", SourceText.From(
                $$"""
                namespace {{model.Namespace}}
                {
                    [global::System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
                    public partial class {{model.ClassName}} : global::{{typeof(IProblemDetails).FullName}}
                    {
                        {{compilerAttributes}}
                        public int Year => {{model.Year}};
                
                        {{compilerAttributes}}
                        public int Day => {{model.Day}};
                
                        {{compilerAttributes}}
                        public string Name => {{(string.IsNullOrWhiteSpace(model.Name) ? "string.Empty" : $"\"{model.Name}\"")}};
                
                        {{compilerAttributes}}
                        public override string ToString() => $"Year {this.Year} Day {this.Day}{(!string.IsNullOrWhiteSpace(this.Name) ? $" - {this.Name}" : string.Empty)}";
                    }
                }
                """,
                Encoding.UTF8));
        });
    }
}
