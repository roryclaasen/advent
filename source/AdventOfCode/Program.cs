using AdventOfCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
    var runners = FindAllOfType<IRunner>(GetAdventAssemblies());
    foreach (var runnerType in runners)
    {
        var runnerObj = Activator.CreateInstance(runnerType);
        if (runnerObj is IRunner runner)
        {
            if (!dictionary.ContainsKey(runner.Year))
            {
                dictionary.Add(runner.Year, new List<IRunner>());
            }

            dictionary[runner.Year].Add(runner);
        }
        else
        {
            // TODO Throw
        }
    }


    return dictionary;
}

var consoleWidth = 64;
var yearSep = new string('#', consoleWidth);

var allRunners = GetRunners();
foreach (var year in allRunners.Keys)
{
    Console.WriteLine(yearSep);
    Console.WriteLine(year.ToString().PadLeft((consoleWidth + 4) / 2));
    Console.WriteLine(yearSep);
    var runners = allRunners[year].OrderBy(r => r.Day);
    foreach (var runner in runners)
    {
        Console.WriteLine($" < Day {runner.Day} >");

        var part1 = await runner.SolvePart1();
        Console.WriteLine($"  Part 1:\n{part1}\n");

        var part2 = await runner.SolvePart2();
        Console.WriteLine($"  Part 2:\n{part2}\n");
    }
}