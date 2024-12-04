namespace AdventOfCode.Commands;

using AdventOfCode.Shared;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

internal class YearAndDaySettings(IDateTimeProvider dateTimeProvider) : YearSettings(dateTimeProvider)
{
    [Description("The day of the the available puzzles to run.")]
    [CommandOption("-d|--day")]
    public int? Day { get; init; }

    public override ValidationResult Validate()
    {
        if (this.Day is not null)
        {
            if (this.Day < 1 || this.Day > 25)
            {
                return ValidationResult.Error("Day must be between 1 and 25.");
            }
        }

        return base.Validate();
    }
}
