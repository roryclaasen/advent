using AdventOfCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

void WriteAnswer(int part, string answer)
{
    Console.WriteLine($"  Part {part}:");
    foreach(var line in answer.Split(Environment.NewLine))
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

        WriteAnswer(1, await runner.SolvePart1().ConfigureAwait(false));
        WriteAnswer(2, await runner.SolvePart2().ConfigureAwait(false));
    }
}