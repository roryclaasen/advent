namespace AdventOfCode.Commands;

using AdventOfCode.Infrastructure;
using AdventOfCode.Shared;
using Spectre.Console;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class TodayCommand(Options options, IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder, Runner solutionRunner)
    : BaseSolutionCommand(options, solutionFinder, solutionRunner, "today", "Run todays solution")
{
    protected override Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var now = dateTimeProvider.AoCNow;
        if (now is { Month: 12, Day: >= 1 and <= 25 })
        {
            var solvers = this.SolutionFinder.GetSolversFor(now.Year, now.Day);
            var allResults = this.SolutionRunner.RunAll(solvers);
            var exitCode = allResults.Any(r => r.HasError) ? -1 : 0;
            return Task.FromResult(exitCode);
        }

        AnsiConsole.MarkupLine($"[{Color.Red}]Error:[/] Event is not active. This option works in Dec 1-25 only.");
        AnsiConsole.MarkupLine("Run --help to see all available commands.");

        return Task.FromResult(-1);
    }
}
