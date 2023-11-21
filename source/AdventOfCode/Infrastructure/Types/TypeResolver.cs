namespace AdventOfCode.Infrastructure;

using Spectre.Console.Cli;
using System;

internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    public object? Resolve(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return provider.GetService(type);
    }

    public void Dispose()
    {
        if (provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
