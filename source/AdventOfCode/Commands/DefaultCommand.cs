namespace AdventOfCode.Commands;

using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AdventOfCode.Infrastructure;
using AdventOfCode.Shared;

internal sealed class DefaultCommand(IDateTimeProvider dateTimeProvider, SolutionFinder solutionFinder, Runner solutionRunner) : Command<DefaultCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("The year of the available puzzles to run.")]
        [CommandOption("-y|--year")]
        public int? Year { get; init; }

        [Description("The day of the the available puzzles to run.")]
        [CommandOption("-d|--day")]
        public int? Day { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var solvers = solutionFinder.GetSolversFor(settings.Year, settings.Day);
        var allResults = solutionRunner.RunAll(solvers);
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

        if (settings.Day is not null)
        {
            if (settings.Day < 1 || settings.Day > 25)
            {
                return ValidationResult.Error("Day must be between 1 and 25.");
            }
        }

        return base.Validate(context, settings);
    }
}
