namespace AdventOfCode.Analyzers;

using AdventOfCode.Problem;
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
                return new ResourceInfo(Namespace, className, fileName, f.GetText(ct)?.ToString());
            });

        context.RegisterSourceOutput(resourcePipeline, static (context, model) => context.AddSource($"{model.ClassName}-{model.FileName}.g.cs", SourceText.From($$""""
            namespace {{model.Namespace}}
            {
                public partial class {{model.ClassName}} : {{model.Interface}}
                {
                    [System.Runtime.CompilerServices.CompilerGenerated]
                    public string {{model.FileName}} =>
                        """
                        {{model.PadContents(12)}}
                        """;
                }
            }
            """", Encoding.UTF8)));
    }

    private readonly record struct ResourceInfo
    {
        public readonly string Namespace;
        public readonly string ClassName;
        public readonly string FileName;
        public readonly string? Contents;

        public ResourceInfo(string Namespace, string className, string fileName, string? contents)
        {
            this.Namespace = Namespace;
            this.ClassName = className;
            this.FileName = fileName;
            this.Contents = contents;
        }

        public string Interface => this.FileName switch
        {
            "Input" => typeof(IProblemInput).FullName,
            "Expected1" => typeof(IProblemExpectedResultPart1).FullName,
            "Expected2" => typeof(IProblemExpectedResultPart2).FullName,
            _ => throw new NotImplementedException()
        };

        public string PadContents(int padding = 0, bool skipFirstLine = true)
        {
            if (this.Contents is null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            var lines = this.Contents.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            var length = lines.Length - 1;
            for (int i = 0; i < lines.Length; i++)
            {
                if ((i == 0 && !skipFirstLine) || i != 0)
                {
                    sb.Append(new string(' ', padding));
                }
                if (i == length)
                {
                    sb.Append(lines[i]);
                }
                else
                {
                    sb.AppendLine(lines[i]);
                }
            }
            return sb.ToString();
        }
    }
}
