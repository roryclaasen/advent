namespace AdventOfCode.Commands;

using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

internal class YearSettings(IDateTimeProvider dateTimeProvider) : CommandSettings
{
    protected readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

    [Description("The year of the available puzzles to list.")]
    [CommandOption("-y|--year")]
    public int? Year { get; init; }

    public override ValidationResult Validate()
    {
        if (this.Year is not null)
        {
            if (this.Year < 2015 || this.Year > this.dateTimeProvider.Now.Year)
            {
                return ValidationResult.Error("Year must be between 2015 and current year.");
            }
        }

        return base.Validate();
    }
}
