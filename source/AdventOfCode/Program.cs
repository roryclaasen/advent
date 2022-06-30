// ------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Infrastructure;

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
         .Where(p => baseType.IsAssignableFrom(p));
}

Dictionary<int, List<IRunner>> GetRunners()
{
    var dictionary = new Dictionary<int, List<IRunner>>();
    var runners = FindAllOfType<IRunner>(GetAdventAssemblies())
        .Select(Activator.CreateInstance)
        .OfType<IRunner>();

    foreach (var runner in runners)
    {
        if (!dictionary.ContainsKey(runner.Year))
        {
            dictionary.Add(runner.Year, new List<IRunner>());
        }

        dictionary[runner.Year].Add(runner);
    }

    return dictionary;
}

void WriteYear(string year)
{
    var consoleWidth = 64;
    var yearSep = new string('#', consoleWidth);

    Console.WriteLine(yearSep);
    Console.WriteLine(year.PadLeft((consoleWidth + 4) / 2));
    Console.WriteLine(yearSep);
}

async Task WriteAnswer(int part, Func<Task<string>> solve)
{
    var stopwatch = new Stopwatch();
    Console.Write($"  Part {part}:");

    stopwatch.Start();
    var answer = await solve().ConfigureAwait(false);
    stopwatch.Stop();

    Console.WriteLine($" ({stopwatch.ElapsedMilliseconds}ms)");
    foreach (var line in answer.Split(Environment.NewLine))
    {
        Console.WriteLine($"    {line}");
    }
}

var allRunners = GetRunners();

foreach (var year in allRunners.Keys)
{
    WriteYear(year.ToString());

    var runners = allRunners[year].OrderBy(r => r.Day);
    foreach (var runner in runners)
    {
        Console.WriteLine($"Day {runner.Day}");

        await WriteAnswer(1, runner.SolvePart1).ConfigureAwait(false);
        await WriteAnswer(2, runner.SolvePart2).ConfigureAwait(false);
    }
}
