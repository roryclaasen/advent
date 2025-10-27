namespace AdventOfCode.Commands;

using System;

using BaseRootCommand = System.CommandLine.RootCommand;

internal sealed class RootCommand : BaseRootCommand
{
    public RootCommand(
        ListCommand listCommand,
        AllCommand allCommand,
        TodayCommand todayCommand,
        LastCommand lastCommand,
        PickCommand pickCommand) : base("Rory Claasens solutions to Advent of Code")
    {
        ArgumentNullException.ThrowIfNull(listCommand);
        ArgumentNullException.ThrowIfNull(allCommand);
        ArgumentNullException.ThrowIfNull(todayCommand);
        ArgumentNullException.ThrowIfNull(lastCommand);
        ArgumentNullException.ThrowIfNull(pickCommand);

        this.Subcommands.Add(listCommand);
        this.Subcommands.Add(allCommand);
        this.Subcommands.Add(todayCommand);
        this.Subcommands.Add(lastCommand);
        this.Subcommands.Add(pickCommand);
    }
}
