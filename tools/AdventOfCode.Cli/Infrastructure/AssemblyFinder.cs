// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Infrastructure;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

internal sealed class AssemblyFinder
{
    private static readonly Lock LoadLock = new();
    private static readonly ConcurrentDictionary<string, Assembly> CachedAssemblies = new();

    public static IEnumerable<Type> FindAllOfType<T>()
    {
        var baseType = typeof(T);

        return GetAdventAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(baseType.IsAssignableFrom);
    }

    private static IEnumerable<Assembly> GetAdventAssemblies()
    {
        var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var moduleFiles = directoryInfo.GetFiles("AdventOfCode.Year*.dll");

        foreach (var file in moduleFiles)
        {
            yield return LoadAssembly(Path.GetFileNameWithoutExtension(file.Name));
        }
    }

    private static Assembly LoadAssembly(string path)
    {
        lock (LoadLock)
        {
            if (CachedAssemblies.TryGetValue(path, out var cachedAssembly))
            {
                return cachedAssembly;
            }

            var assembly = Assembly.Load(path);
            CachedAssemblies[path] = assembly;
            return assembly;
        }
    }
}
