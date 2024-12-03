namespace AdventOfCode.Commands;

using Spectre.Console.Cli;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AdventOfCode.Infrastructure;
using AdventOfCode.Shared;

internal sealed class TodayCommand(IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder, Runner solutionRunner) : Command
{
    public override int Execute([NotNull] CommandContext context)
    {
        var now = dateTimeProvider.AoCNow;
        if (now is { Month: 12, Day: >= 1 and <= 25 })
        {
            var solvers = solutionFinder.GetSolversFor(now.Year, now.Day);
            var allResults = solutionRunner.RunAll(solvers);
            var isError = allResults.Any(r => r.HasError);
            return isError ? -1 : 0;
        }

        AnsiConsole.MarkupLine($"[{Color.Red}]Error:[/] Event is not active. This option works in Dec 1-25 only.");
        AnsiConsole.MarkupLine("Run --help to see all available commands.");

        return -1;
    }
}
