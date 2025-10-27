// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2024;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;

[Problem(2024, 9, "Disk Fragmenter")]
public partial class Day9Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var (diskMap, _, _) = ParseInput(input);
        for (var i = diskMap.Count - 1; i > 0; i--)
        {
            var currentBlock = diskMap[i];
            if (currentBlock == -1)
            {
                continue;
            }

            var freeBlock = diskMap.IndexOf(-1);
            if (freeBlock >= i)
            {
                break;
            }

            diskMap[freeBlock] = currentBlock;
            diskMap[i] = -1;
        }

        return diskMap.WithIndex().Where(d => d.Item > 0).Sum(d => d.Index * d.Item * 1L);
    }

    public object? PartTwo(string input)
    {
        var (compressed, reserved, freeBlocks) = ParseInput(input);
        foreach (var file in reserved)
        {
            var freeSpace = freeBlocks.FirstOrDefault(block => block.Index < file.Index && block.Length >= file.Length);
            if (freeSpace == default)
            {
                continue;
            }

            for (var i = 0; i < file.Length; i++)
            {
                compressed[freeSpace.Index + i] = file.Id;
                compressed[file.Index + i] = -1;
            }

            if (freeSpace.Length == file.Length)
            {
                freeBlocks.Remove(freeSpace);
            }
            else
            {
                freeBlocks[freeBlocks.IndexOf(freeSpace)] = new(freeSpace.Index + file.Length, freeSpace.Length - file.Length);
            }
        }

        return compressed.WithIndex().Where(d => d.Item > 0).Sum(d => d.Index * d.Item * 1L);
    }

    private static (List<int> DiskMap, IReadOnlyList<Reserved> Resserved, List<FreeBlock> FreeBlocks) ParseInput(string input)
    {
        var diskMap = new List<int>();
        var filesReversed = new List<Reserved>();
        var freeBlocks = new List<FreeBlock>();

        for (var i = 1; i <= input.Length; i += 2)
        {
            var fileLength = input[i - 1] - '0';
            var freeSpace = i == input.Length ? 0 : input[i] - '0';

            filesReversed.Insert(0, new(i / 2, diskMap.Count, fileLength));
            freeBlocks.Add(new(diskMap.Count + fileLength, freeSpace));

            diskMap.AddRange(Enumerable.Repeat(i / 2, fileLength));
            diskMap.AddRange(Enumerable.Repeat(-1, freeSpace));
        }

        return (diskMap, filesReversed, freeBlocks);
    }

    private record struct Reserved(int Id, int Index, int Length);

    private record struct FreeBlock(int Index, int Length);
}
