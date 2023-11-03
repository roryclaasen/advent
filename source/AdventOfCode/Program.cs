using AdventOfCode;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
         .Where(p => baseType.IsAssignableFrom(p));
}

IEnumerable<ISolver> GetSolvers(params Type[] solverTypes) => solverTypes.Select(Activator.CreateInstance).OfType<ISolver>();

var allSolverTypes = FindAllOfType<ISolver>(GetAdventAssemblies()).ToArray();

Func<Task>? Command(string[] args, string[] regexes, Func<string[], Func<Task>> parse)
{
    if (args.Length != regexes.Length)
    {
        return null;
    }

    var matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
    if (!matches.All(match => match.Success))
    {
        return null;
    }

    try
    {
        var matchedArgs = matches.SelectMany(m => m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) : new[] { m.Value }).ToArray();
        return parse(matchedArgs);
    }
    catch
    {
        return null;
    }
}

string[] Args(params string[] regex) => regex;

var job =
    Command(args, Args("([0-9]+)/(Day)?([0-9]+)"), m =>
    {
        var year = int.Parse(m[0]);
        var day = int.Parse(m[2]);
        var solverType = allSolverTypes.First(tSolver => ISolverExtensions.Year(tSolver) == year && ISolverExtensions.Day(tSolver) == day);
        return () => Runner.RunAll(GetSolvers(solverType));
    }) ??
    Command(args, Args("[0-9]+"), m =>
    {
        var year = int.Parse(m[0]);
        var solverType = allSolverTypes.Where(tSolver => ISolverExtensions.Year(tSolver) == year).ToArray();
        return () => Runner.RunAll(GetSolvers(solverType));
    }) ??
    Command(args, Args("([0-9]+)/all"), m =>
    {
        var year = int.Parse(m[0]);
        var solverType = allSolverTypes.Where(tSolver => ISolverExtensions.Year(tSolver) == year).ToArray();
        return () => Runner.RunAll(GetSolvers(solverType));
    }) ??
    Command(args, Args("all"), m =>
    {
        return () => Runner.RunAll(GetSolvers(allSolverTypes));
    }) ??
    Command(args, Args("today"), m =>
    {
        var dt = DateTime.UtcNow.AddHours(-5);
        if (dt is { Month: 12, Day: >= 1 and <= 25 })
        {
            var solversTypesSelected = allSolverTypes.First(tsolver =>
                ISolverExtensions.Year(tsolver) == dt.Year &&
                ISolverExtensions.Day(tsolver) == dt.Day);

            return () => Runner.RunAll(GetSolvers(solversTypesSelected));
        }
        else
        {
            throw new Exception("Event is not active. This option works in Dec 1-25 only)");
        }
    }) ??
    (() => Task.Run(() =>
    {
        Console.WriteLine("Help text");
    }));

await job().ConfigureAwait(false);
