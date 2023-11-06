namespace AdventOfCode;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public static class SolutionFinder
{
    private static IEnumerable<Assembly> GetAdventAssemblies()
    {
        var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var moduleFiles = directoryInfo.GetFiles("AdventOfCode.*.dll");

        foreach (var file in moduleFiles)
        {
            yield return Assembly.Load(Path.GetFileNameWithoutExtension(file.Name));
        }
    }

    private static IEnumerable<Type> FindAllOfType<T>(IEnumerable<Assembly> assemblies)
    {
        var baseType = typeof(T);

        return assemblies.SelectMany(a => a.GetTypes())
             .Where(t => t.IsClass && !t.IsAbstract)
             .Where(baseType.IsAssignableFrom);
    }

    public static IEnumerable<Type> GetSolvers(int? year = null, int? day = null)
    {
        var allISolversTypes = FindAllOfType<ISolver>(GetAdventAssemblies());

        if (day is not null && year is not null)
        {
            var solverType = allISolversTypes.Where(tSolver => GetYearFromType(tSolver) == year && GetDayFromType(tSolver) == day);
            if (solverType.Any())
            {
                return solverType;
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year} and day {day}", year, day);
        }
        else if (day is null && year is not null)
        {
            var solverType = allISolversTypes.Where(tSolver => GetYearFromType(tSolver) == year);
            if (solverType.Any())
            {
                return solverType;
            }

            throw new SolutionMissingException($"There are no solutions yet for the year {year}", year);
        }
        else if (day is not null && year is null)
        {
            var solverType = allISolversTypes.Where(tSolver => GetDayFromType(tSolver) == day);
            if (solverType.Any())
            {
                return solverType;
            }

            throw new SolutionMissingException($"There are no solutions yet for the day {day}", day: day);
        }

        return allISolversTypes;
    }

    public static IEnumerable<Type> GetLastSolvers(int? year = null)
        => GetSolvers(year)
            .GroupBy(GetYearFromType)
            .Select(year => year.OrderBy(GetDayFromType).Last());

    private static int GetYearFromType(Type type)
        => int.Parse(type.FullName!.Split('.')[1][4..]);

    private static int GetDayFromType(Type type)
        => int.Parse(type.FullName!.Split('.')[2][3..^8]);
}
