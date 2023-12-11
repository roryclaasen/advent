namespace AdventOfCode.Year2022;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2022, 7, "No Space Left On Device")]
public class Day7Solution : ISolver
{
    public object? PartOne(string input)
    {
        var total = 0;

        DirectoryIterator(ParseInput(input), (dir) =>
        {
            var size = dir.Size;
            if (size <= 100000)
            {
                total += size;
            }
        });

        return total;
    }

    public object? PartTwo(string input)
    {
        var parsedInput = ParseInput(input);

        var currentFree = 70000000 - parsedInput.Size;
        var neededSize = 30000000;
        var possibleDirs = new List<Directory>();

        DirectoryIterator(parsedInput, (dir) =>
        {
            var size = dir.Size;
            if (currentFree + size >= neededSize)
            {
                possibleDirs.Add(dir);
            }
        });

        return possibleDirs.OrderBy(d => d.Size).First().Size;
    }

    private static void DirectoryIterator(Directory dir, Action<Directory> action)
    {
        foreach (var (_, node) in dir.Nodes)
        {
            if (node is Directory nodeDir)
            {
                DirectoryIterator(nodeDir, action);
            }
        }

        action(dir);
    }

    private static Directory ParseInput(string input)
    {
        var cmdRegex = new Regex(@"^\$ (cd|ls)(?: (\w+|..|\/))?$");
        var listRegex = new Regex(@"^(dir|\d+) (\w+\.?\w*)$");

        var lines = new Queue<string>(input.Lines());
        Directory? tree = null;
        Directory? currentDir = null;
        while (lines.Count > 0)
        {
            var instruction = lines.Dequeue();
            var insRegex = cmdRegex.Match(instruction);
            var ins = insRegex.Groups[1].Value;
            if (ins == "cd")
            {
                var dir = insRegex.Groups[2].Value;
                if (dir == "..")
                {
                    if (currentDir?.Parent is Directory parent)
                    {
                        currentDir = parent;
                    }
                    else
                    {
                        throw new Exception("Unable to go back a layer");
                    }
                }
                else
                {
                    if (currentDir is null)
                    {
                        var node = new Directory(null!, dir);
                        tree = node;
                        currentDir = node;
                    }
                    else
                    {
                        if (currentDir.Nodes.TryGetValue(dir, out var node))
                        {
                            if (node is Directory directory)
                            {
                                currentDir = directory;
                            }
                            else
                            {
                                throw new Exception("Node is not a directory");
                            }
                        }
                        else
                        {
                            var newNode = new Directory(currentDir, dir);
                            currentDir.Nodes.Add(dir, newNode);
                            currentDir = newNode;
                        }
                    }
                }
            }
            else if (ins == "ls")
            {
                if (currentDir is null)
                {
                    throw new Exception("Current directory is null");
                }

                while (lines.Count > 0 && !lines.Peek().StartsWith("$"))
                {
                    var output = lines.Dequeue();
                    var outRegex = listRegex.Match(output);
                    var first = outRegex.Groups[1].Value;
                    var second = outRegex.Groups[2].Value;
                    Node? node;
                    if (first == "dir")
                    {
                        node = new Directory(currentDir, second);
                    }
                    else
                    {
                        node = new Node(currentDir, second, int.Parse(first));
                    }

                    if (node is null)
                    {
                        throw new Exception("Unable to parse node");
                    }

                    currentDir.Nodes.Add(second, node);
                }
            }
            else
            {
                throw new Exception($"Unknown command {ins}");
            }
        }

        if (tree is null)
        {
            throw new Exception("Unable to parse tree");
        }

        return tree;
    }

    private record Node(Node Parent, string Name, int Size)
    {
        public virtual int Size { get; init; } = Size;
    }

    private record Directory(Node Parent, string Name) : Node(Parent, Name, default)
    {
        public Dictionary<string, Node> Nodes { get; private set; } = new();

        public override int Size => this.Nodes.Values.Sum(f => f.Size);
    }
}
