namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal sealed class AllCommand(SolutionFinder solutionFinder, Runner solutionRunner) : Command<YearAndDaySettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] YearAndDaySettings settings)
    {
        var solvers = solutionFinder.GetSolversFor(settings.Year, settings.Day);
        var allResults = solutionRunner.RunAll(solvers);
        var isError = allResults.Any(r => r.HasError);
        return isError ? -1 : 0;
    }
}
