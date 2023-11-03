namespace AdventOfCode.Shared;

using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

public static class AssemblyExtensions
{
    private static readonly ConcurrentDictionary<Assembly, InputReader> CachedInputReaders = new();

    public static Task<string> ReadResourceFile(this Assembly assembly, string filePath)
    {
        var inputReader = CachedInputReaders.GetOrAdd(assembly, a => new InputReader(a));
        return inputReader.ReadFile(filePath);
    }
}
