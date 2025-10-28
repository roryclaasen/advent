// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using AdventOfCode.Cli.Commands;
using AdventOfCode.Cli.Services;
using AdventOfCode.Cli.Services.Finder;
using AdventOfCode.Cli.Services.Runner;
using AdventOfCode.Problem;
using AdventOfCode.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.Text;

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

    foreach (var solution in AssemblyFinder.FindAllOfType<IProblemSolver>())
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IProblemSolver), solution));
    }

    builder.Services.AddSingleton<Options>();

    builder.Services.AddTransient<AdventOfCode.Cli.Commands.RootCommand>();
    builder.Services.AddTransient<ListCommand>();
    builder.Services.AddTransient<AllCommand>();
    builder.Services.AddTransient<TodayCommand>();
    builder.Services.AddTransient<LastCommand>();
    builder.Services.AddTransient<PickCommand>();

    var app = builder.Build();
    return app;
}
