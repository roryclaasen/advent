// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using System.Linq;
using System.Text;
using AdventOfCode.Problem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ProblemGenerator : IIncrementalGenerator
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
            context.AddSource($"{model.ClassName}.g.cs", SourceText.From(
                $$"""
                namespace {{model.Namespace}}
                {
                    [global::System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
                    public partial class {{model.ClassName}} : global::{{typeof(IProblemDetails).FullName}}
                    {
                        {{CompilerAttributes}}
                        public int Year => {{model.Year}};
                
                        {{CompilerAttributes}}
                        public int Day => {{model.Day}};
                
                        {{CompilerAttributes}}
                        public string Name => {{(string.IsNullOrWhiteSpace(model.Name) ? "string.Empty" : $"\"{model.Name}\"")}};
                
                        {{CompilerAttributes}}
                        public override string ToString() => $"Year {this.Year} Day {this.Day}{(!string.IsNullOrWhiteSpace(this.Name) ? $" - {this.Name}" : string.Empty)}";
                    }
                }
                """,
                Encoding.UTF8));
        });

        var servicesPipeline = pipeline.Collect();
        context.RegisterSourceOutput(servicesPipeline, static (context, models) =>
        {
            var exampleProblem = models.FirstOrDefault();
            if (exampleProblem == default)
            {
                return;
            }

            var problemType = typeof(IProblemSolver).FullName;
            var tryAddEnumerableLines = models
                .OrderBy(model => model.Day)
                .Select(model => $"typeof(global::{model.Namespace}.{model.ClassName})")
                .Select(model => $"services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(global::{problemType}), {model}));");

            context.AddSource($"ServiceCollectionExtensions.g.cs", SourceText.From(
                $$"""
                namespace {{exampleProblem.Namespace}}
                {
                    using global::Microsoft.Extensions.DependencyInjection;
                    using global::Microsoft.Extensions.DependencyInjection.Extensions;
                
                    public static class ServiceCollectionExtensions
                    {
                        {{CompilerAttributes}}
                        public static IServiceCollection AddSolutionsFor{{exampleProblem.Year}}(this IServiceCollection services)
                        {
                            {{string.Join($"\r\n            ", tryAddEnumerableLines)}}
                            return services;
                        }
                    }
                }
                """,
                Encoding.UTF8));
        });
    }
}
