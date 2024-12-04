using AdventOfCode.Commands;
using AdventOfCode.Infrastructure;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using System;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var registrations = new ServiceCollection();
registrations.AddSingleton<IDateTimeProvider, DateTimeProvider>();
registrations.AddSingleton<AdventUri>();
registrations.AddSingleton<SolutionFinder>();
registrations.AddSingleton<Runner>();

foreach (var solution in AssemblyFinder.FindAllOfType<IProblemSolver>())
{
    registrations.AddSingleton(typeof(IProblemSolver), solution);
}

var registrar = new TypeRegistrar(registrations);
var app = new CommandApp<DefaultCommand>(registrar);
app.WithDescription("Rory Claasens solutions to Advent of Code");
app.Configure(config =>
{
    config.SetApplicationName(ThisAssembly.AssemblyName + ".exe");
    config.SetApplicationVersion(ThisAssembly.AssemblyVersion);

#if DEBUG
    config.ValidateExamples();
    config.PropagateExceptions();
#endif

    config.AddExample([]);
    config.AddExample(["--year", "2015"]);

    config.AddCommand<ListCommand>("list")
        .WithDescription("List all available solutions")
        .WithExample(["list"])
        .WithExample(["list", "--year", "2020"]);

    config.AddCommand<TodayCommand>("today")
        .WithDescription("Run todays solution");

    config.AddCommand<LastCommand>("last")
        .WithDescription("Run the last solution for a given year")
        .WithExample(["last"])
        .WithExample(["last", "--year", "2022"]);

    config.AddCommand<AllCommand>("all")
        .WithDescription("Run all the solutions")
        .WithExample(["all"])
        .WithExample(["all", "--day", "1"])
        .WithExample(["all", "--year", "2024"])
        .WithExample(["all", "--year", "2020", "--day", "12"]);
});

var exitCode = await app.RunAsync(args).ConfigureAwait(false);
return exitCode;
