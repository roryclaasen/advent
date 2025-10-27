namespace AdventOfCode.Commands;

using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

internal abstract class BaseCommand : Command
{
    public BaseCommand(string name, string? description = null) : base(name, description)
    {
        this.SetAction(ExecuteAsync);
    }

    protected abstract Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken);
}
