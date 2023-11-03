namespace AdventOfCode.Shared;

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

internal class InputReader
{
    private readonly EmbeddedFileProvider fileProvider;

    public InputReader(Assembly assembly)
    {
        this.fileProvider = new EmbeddedFileProvider(assembly);
    }

    public Task<string> ReadFile(string filePath)
    {
        using var stream = this.GetStream(filePath);
        using var reader = new StreamReader(stream);
        return reader.ReadToEndAsync();
    }

    private Stream GetStream(string filePath)
    {
        var fileInfo = this.fileProvider.GetFileInfo(filePath);
        if (fileInfo is null || !fileInfo.Exists)
        {
            throw new FileNotFoundException($"Unable to read file {filePath} from assembly");
        }

        return fileInfo.CreateReadStream();
    }
}
