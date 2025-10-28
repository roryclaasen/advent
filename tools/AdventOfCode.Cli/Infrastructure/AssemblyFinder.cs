// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Cli.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

internal sealed class AssemblyFinder
{
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
            yield return Assembly.Load(Path.GetFileNameWithoutExtension(file.Name));
        }
    }
}
