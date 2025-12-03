// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.CommandLine;
using System.Text;
using AdventOfCode.Cli.Commands;
using AdventOfCode.Cli.Services;
using AdventOfCode.Cli.Services.Finder;
using AdventOfCode.Cli.Services.Runner;
using AdventOfCode.Shared;
using AdventOfCode.Year2015;
using AdventOfCode.Year2016;
using AdventOfCode.Year2017;
using AdventOfCode.Year2018;
using AdventOfCode.Year2019;
using AdventOfCode.Year2020;
using AdventOfCode.Year2022;
using AdventOfCode.Year2023;
using AdventOfCode.Year2024;
using AdventOfCode.Year2025;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.OutputEncoding = Encoding.UTF8;

using var app = BuildApplicationAsync(args);

await app.StartAsync().ConfigureAwait(false);

var rootCommand = app.Services.GetRequiredService<AdventOfCode.Cli.Commands.RootCommand>();

var invokeConfig = new InvocationConfiguration()
{
#if DEBUG
    EnableDefaultExceptionHandler = !System.Diagnostics.Debugger.IsAttached
#else
    EnableDefaultExceptionHandler = true
#endif
};

var exitCode = await rootCommand.Parse(args).InvokeAsync(invokeConfig);

await app.StopAsync().ConfigureAwait(false);

return exitCode;

static IHost BuildApplicationAsync(string[] args)
{
    var settings = new HostApplicationBuilderSettings
    {
        Configuration = new ConfigurationManager()
    };
    settings.Configuration.AddEnvironmentVariables();

    var builder = Host.CreateEmptyApplicationBuilder(settings);
    builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    builder.Services.AddSingleton<AdventUri>();
    builder.Services.AddSingleton<ISolutionFinder, SolutionFinder>();
    builder.Services.AddSingleton<ISolutionRunner, SolutionRunner>();

    builder.Services.AddSolutionsFor2015();
    builder.Services.AddSolutionsFor2016();
    builder.Services.AddSolutionsFor2017();
    builder.Services.AddSolutionsFor2018();
    builder.Services.AddSolutionsFor2019();
    builder.Services.AddSolutionsFor2020();
    builder.Services.AddSolutionsFor2022();
    builder.Services.AddSolutionsFor2023();
    builder.Services.AddSolutionsFor2024();
    builder.Services.AddSolutionsFor2025();

    builder.Services.AddTransient<AdventOfCode.Cli.Commands.RootCommand>();
    builder.Services.AddTransient<ListCommand>();
    builder.Services.AddTransient<AllCommand>();
    builder.Services.AddTransient<TodayCommand>();
    builder.Services.AddTransient<LastCommand>();

    return builder.Build();
}
