namespace AdventOfCode.Analyzers;

using AdventOfCode.Problem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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
            {{FileHeaderComment}}

            namespace {{model.Namespace}}
            {
                [System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
                public partial class {{model.ClassName}} : {{typeof(IProblemDetails).FullName}}
                {
                    public int Year => {{model.Year}};

                    public int Day => {{model.Day}};

                    public string Name => {{(string.IsNullOrWhiteSpace(model.Name) ? "string.Empty" : $"\"{model.Name}\"")}};

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
