// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Analyzers;

using System.Collections.Immutable;
using System.Linq;
using System.Text;
using AdventOfCode.Problem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ProblemGenerator : IIncrementalGenerator
{
    private static readonly SymbolDisplayFormat NamespaceFormat =
        SymbolDisplayFormat.FullyQualifiedFormat
            .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var perProblem = context.SyntaxProvider.ForAttributeWithMetadataName(
            typeof(ProblemAttribute).FullName,
            predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax,
            transform: static (context, _) =>
            {
                var containingClass = context.TargetSymbol;
                var attribute = context.Attributes[0];
                return new ProblemInfo(
                    containingClass.ContainingNamespace.ToDisplayString(NamespaceFormat),
                    containingClass.Name,
                    (int)attribute.ConstructorArguments[0].Value!,
                    (int)attribute.ConstructorArguments[1].Value!,
                    attribute.ConstructorArguments.Length == 3 ? attribute.ConstructorArguments[2].Value?.ToString() : null);
            });

        // Emit one Day{N}Solution.g.cs per problem. Keeping this stream un-collected means that
        // editing a single [Problem]-attributed class only regenerates that class's file, instead
        // of invalidating every generated file in the project.
        context.RegisterSourceOutput(perProblem, static (context, model) => EmitProblemDetails(context, model));

        // The service-collection extensions file genuinely needs the full set of problems; this
        // single aggregated file is the only output that regenerates when any problem changes.
        context.RegisterSourceOutput(perProblem.Collect(), static (context, models) => EmitServiceCollectionExtensions(context, models));
    }

    private static void EmitProblemDetails(SourceProductionContext context, ProblemInfo model)
    {
        context.AddSource($"{model.ClassName}.g.cs", SourceText.From(
            $$"""
            namespace {{model.Namespace}}
            {
                [global::System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
                public partial class {{model.ClassName}} : global::{{typeof(IProblemDetails).FullName}}
                {
                    {{GeneratorConstants.CompilerAttributes}}
                    public int Year => {{model.Year}};

                    {{GeneratorConstants.CompilerAttributes}}
                    public int Day => {{model.Day}};

                    {{GeneratorConstants.CompilerAttributes}}
                    public string Name => {{(string.IsNullOrWhiteSpace(model.Name) ? "string.Empty" : $"\"{model.Name}\"")}};

                    {{GeneratorConstants.CompilerAttributes}}
                    public override string ToString() => $"Year {this.Year} Day {this.Day}{(!string.IsNullOrWhiteSpace(this.Name) ? $" - {this.Name}" : string.Empty)}";
                }
            }
            """,
            Encoding.UTF8));
    }

    private static void EmitServiceCollectionExtensions(SourceProductionContext context, ImmutableArray<ProblemInfo> models)
    {
        if (models.IsDefaultOrEmpty)
        {
            return;
        }

        var exampleProblem = models[0];
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
                        /// <summary>
                        /// Registers all Advent of Code {{exampleProblem.Year}} problem solvers as <see cref="global::{{problemType}}"/> singletons.
                        /// </summary>
                        /// <param name="services">The <see cref="IServiceCollection"/> to add the solvers to.</param>
                        /// <returns>The same <see cref="IServiceCollection"/> so that calls can be chained.</returns>
                        {{GeneratorConstants.CompilerAttributes}}
                        public static IServiceCollection AddSolutionsFor{{exampleProblem.Year}}(this IServiceCollection services)
                        {
                            {{string.Join($"\r\n            ", tryAddEnumerableLines)}}
                            return services;
                        }
                    }
                }
                """,
            Encoding.UTF8));
    }
}
