// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2016;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2016, 4, "Security Through Obscurity")]
public partial class Day4Solution : IProblemSolver
{
    public object? PartOne(string input)
        => ParseRooms(input).Where(this.IsRealRoom).Sum(r => r.SectorId);

    public object? PartTwo(string input)
    {
        foreach (var room in ParseRooms(input).Where(this.IsRealRoom))
        {
            var rotatedName = RotateRoomName(room.Name, room.SectorId);
            if (rotatedName.Contains("northpole"))
            {
                return room.SectorId;
            }
        }

        throw new Exception("Failed to find room");
    }

    private bool IsRealRoom(Room room)
    {
        var letterCounts = room.Name.Where(c => c != '-').GroupBy(c => c).Select(g => (g.Key, g.Count())).OrderByDescending(g => g.Item2).ThenBy(g => g.Key).Take(5).ToArray();
        var checksum = new string([.. letterCounts.Select(g => g.Key)]);
        return checksum == room.Checksum;
    }

    private static string RotateRoomName(string name, int sectorId)
    {
        var rotated = new char[name.Length];
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (c == '-')
            {
                rotated[i] = ' ';
            }
            else
            {
                var offset = (c - 'a' + sectorId) % 26;
                rotated[i] = (char)('a' + offset);
            }
        }

        return new string(rotated);
    }

    private static IEnumerable<Room> ParseRooms(string input)
    {
        foreach (var line in input.Lines())
        {
            yield return Room.Parse(line);
        }
    }

    private partial record Room(string Name, int SectorId, string Checksum)
    {
        public static Room Parse(string input)
        {
            var match = RoomRegex().Match(input);
            if (!match.Success)
            {
                throw new Exception($"Failed to parse room: {input}");
            }

            return new Room(match.Groups["name"].Value, int.Parse(match.Groups["sectorId"].Value), match.Groups["checksum"].Value);
        }

        [GeneratedRegex(@"^(?<name>[a-z-]+)-(?<sectorId>\d+)\[(?<checksum>[a-z]+)\]$")]
        private static partial Regex RoomRegex();
    }
}
