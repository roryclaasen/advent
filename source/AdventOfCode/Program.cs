using AdventOfCode;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

Console.OutputEncoding = Encoding.UTF8;

IEnumerable<Assembly> GetAdventAssemblies()
{
    var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
    var moduleFiles = directoryInfo.GetFiles("AdventOfCode.*.dll");

    foreach (var file in moduleFiles)
    {
        yield return Assembly.Load(Path.GetFileNameWithoutExtension(file.Name));
    }
}

IEnumerable<Type> FindAllOfType<T>(IEnumerable<Assembly> assemblies)
{
    var baseType = typeof(T);

    return assemblies.SelectMany(a => a.GetTypes())
         .Where(t => t.IsClass && !t.IsAbstract)
         .Where(baseType.IsAssignableFrom);
}

IEnumerable<Type> GetSolverTypesFor(int? year = null, int? day = null)
{
    var allSolverTypes = FindAllOfType<ISolver>(GetAdventAssemblies());

    if (day is not null && year is not null)
    {
        var solverType = allSolverTypes.Where(tSolver => ISolverExtensions.Year(tSolver) == year && ISolverExtensions.Day(tSolver) == day);
        if (!solverType.Any())
        {
            Console.WriteLine($"There are no solutions yet for the year {year} and day {day}");
            return Enumerable.Empty<Type>();
        }

        return solverType;
    }
    else if (day is null && year is not null)
    {
        var solverType = allSolverTypes.Where(tSolver => ISolverExtensions.Year(tSolver) == year);
        if (!solverType.Any())
        {
            Console.WriteLine($"There are no solutions yet for the year {year}");
            return Enumerable.Empty<Type>();
        }

        return solverType;
    }
    else if (day is not null && year is null)
    {
        var solverType = allSolverTypes.Where(tSolver => ISolverExtensions.Day(tSolver) == day);
        if (!solverType.Any())
        {
            Console.WriteLine($"There are no solutions yet for the day {day}");
            return Enumerable.Empty<Type>();
        }

        return solverType;
    }

    return allSolverTypes;
}

static IEnumerable<ISolver> CreateSolvers(IEnumerable<Type> solverTypes) => solverTypes.Select(Activator.CreateInstance).OfType<ISolver>();

var AoCDateTime = DateTime.UtcNow.AddHours(-5);

var todayCommand = new Command("today", "Run todays solution, if the event is active.");
todayCommand.SetHandler(() =>
{
    if (AoCDateTime is { Month: 12, Day: >= 1 and <= 25 })
    {
        return Runner.RunAll(CreateSolvers(GetSolverTypesFor(AoCDateTime.Year, AoCDateTime.Day)));
    }

    Console.WriteLine("Event is not active. This option works in Dec 1-25 only.");
    Console.WriteLine("Run --help to see all available commands.");

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
lastCommand.SetHandler((year) =>
{
    var solvers = GetSolverTypesFor(year)
        .GroupBy(t => ISolverExtensions.Year(t))
        .Select(year => year.OrderBy(s => ISolverExtensions.Day(s)).Last());
    return Runner.RunAll(CreateSolvers(solvers));
}, yearOption);

var rootCommand = new RootCommand("Rory Claasens solutions and answers to Advent of Code")
{
    yearOption,
    dayOption,
    todayCommand,
    lastCommand
};
rootCommand.SetHandler((year, day) => Runner.RunAll(CreateSolvers(GetSolverTypesFor(year, day))), yearOption, dayOption);

await rootCommand.InvokeAsync(args).ConfigureAwait(false);
