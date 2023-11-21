namespace AdventOfCode;

using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Infrastructure;
using AdventOfCode.Shared;

internal sealed class LastCommand(IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder, Runner solutionRunner) : AsyncCommand<LastCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("The year of the last available puzzle to run.")]
        [CommandOption("-y|--year")]
        public int? Year { get; init; }
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var solver = solutionFinder.GetLastSolver(settings.Year);
        var allResults = await solutionRunner.RunAll([solver]).ConfigureAwait(false);
        var isError = allResults.Any(r => r.HasError);
        return isError ? -1 : 0;
    }

    public override ValidationResult Validate([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.Year is not null)
        {
            if (settings.Year < 2015 || settings.Year > dateTimeProvider.Now.Year)
            {
                return ValidationResult.Error("Year must be between 2015 and current year.");
            }
        }

        return base.Validate(context, settings);
    }
}
