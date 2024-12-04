namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal sealed class LastCommand(SolutionFinder solutionFinder, Runner solutionRunner) : Command<YearSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] YearSettings settings)
    {
        var solver = solutionFinder.GetLastSolver(settings.Year);
        var allResults = solutionRunner.RunAll([solver]);
        var isError = allResults.Any(r => r.HasError);
        return isError ? -1 : 0;
    }
}
