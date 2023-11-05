using AdventOfCode;
using AdventOfCode.Shared;
using Spectre.Console;
using System;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

Console.OutputEncoding = Encoding.UTF8;

static async Task<int> GetSolutionsAndRun(int? year = null, int? day = null)
{
    try
    {
        var solvers = SolutionFinder.GetSolvers(year, day).Select(Activator.CreateInstance).OfType<ISolver>();
        await Runner.RunAll(solvers).ConfigureAwait(false);
    }
    catch (SolutionMissingException ex)
    {
        AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        return -1;
    }
    catch(Exception ex)
    {
        AnsiConsole.WriteException(ex);
        return -1;
    }

    return 0;
}

var AoCDateTime = DateTime.UtcNow.AddHours(-5);

var todayCommand = new Command("today", "Run todays solution, if the event is active.");
todayCommand.SetHandler(() =>
{
    if (AoCDateTime is { Month: 12, Day: >= 1 and <= 25 })
    {
        return GetSolutionsAndRun(AoCDateTime.Year, AoCDateTime.Day);
    }

    AnsiConsole.MarkupLine("[red]Event is not active. This option works in Dec 1-25 only.[/]");
    AnsiConsole.MarkupLine("Run --help to see all available commands.");

    return Task.FromResult(-1);
});

var yearOption = new Option<int?>
    (aliases: new[] { "--year", "-y" },
    description: "Run the solutions that match the year.",
    parseArgument: result =>
    {
        if (int.TryParse(result.Tokens.Single().Value, out var year))
        {
            var maxYear = AoCDateTime.Month == 12 ? AoCDateTime.Year : AoCDateTime.Year - 1;
            if (year >= 2015 && year <= maxYear)
            {
                return year;
            }
            else
            {
                result.ErrorMessage = "Year must be between 2015 and " + maxYear;
            }
        }
        else
        {
            result.ErrorMessage = "Year must be an integer";
        }

        return null;
    });


var dayOption = new Option<int?>
    (aliases: new[] { "--day", "-d" },
    description: "Run the solutions that match the day.",
    parseArgument: result =>
    {
        if (int.TryParse(result.Tokens.Single().Value, out var day))
        {
            if (day >= 1 && day <= 25)
            {
                return day;
            }
            else
            {
                result.ErrorMessage = "Day must be between 1 and 25";
            }
        }
        else
        {
            result.ErrorMessage = "Day must be an integer";
        }

        return null;
    });

var lastOption = new Option<bool>
    (aliases: new[] { "--last" },
    description: "Run the last solution for the selected year",
    getDefaultValue: () => false);

var lastCommand = new Command("last", "Runs the last solution for each year, unless the year is specifieds.")
{
    yearOption
};
lastCommand.SetHandler((year) => GetSolutionsAndRun(year), yearOption);

var rootCommand = new RootCommand("Rory Claasens solutions and answers to Advent of Code")
{
    yearOption,
    dayOption,
    todayCommand,
    lastCommand
};
rootCommand.SetHandler(GetSolutionsAndRun, yearOption, dayOption);

var exitCode = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
return exitCode;
