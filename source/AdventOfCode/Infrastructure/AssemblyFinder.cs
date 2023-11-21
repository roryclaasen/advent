namespace AdventOfCode.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

internal sealed class AssemblyFinder
{
    private static IEnumerable<Assembly> GetAdventAssemblies()
    {
        var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var moduleFiles = directoryInfo.GetFiles("AdventOfCode.Year*.dll");

        foreach (var file in moduleFiles)
        {
            yield return Assembly.Load(Path.GetFileNameWithoutExtension(file.Name));
        }
    }

    public static IEnumerable<Type> FindAllOfType<T>()
    {
        var baseType = typeof(T);

        return GetAdventAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(baseType.IsAssignableFrom);
    }
}
