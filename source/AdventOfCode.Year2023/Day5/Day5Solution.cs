namespace AdventOfCode.Year2023;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

[Problem(2023, 5, "If You Give A Seed A Fertilizer")]
public class Day5Solution : ISolver
{
    public object? PartOne(string input)
    {
        var parsedInput = ParseInput(input);

        long GetLocation(long source)
        {
            var lastLocation = source;
            foreach (var current in parsedInput.Maps)
            {
                lastLocation = GetNextValue(current, lastLocation);
            }
            return lastLocation;
        }

        static long GetNextValue(List<MapRange> range, long source)
        {
            foreach (var mapRange in range)
            {
                if (mapRange.TryGetDestination(source, out var destination))
                {
                    return destination.Value;
                }
            }

            return source;
        }

        return parsedInput.Seeds.Select(GetLocation).Min();
    }

    public object? PartTwo(string input)
    {
        var parsedInput = ParseInput(input);
        var seeds = parsedInput.Seeds;

        var bag = new TreeNode(0, long.MaxValue);
        for (var i = 0; i < seeds.Length; i += 2)
        {
            var node  = new TreeNode(seeds[i], seeds[i] + seeds[i + 1] - 1);
            node.Populate(parsedInput.Maps);
            bag.Children.Add(node);
        }

        return bag
            .Flatten()
            .Where(n => n.Depth >= parsedInput.Maps.Count)
            .Min(n => n.Start);
    }

    Input ParseInput(string input)
    {
        var parts = input.Lines(2);
        var seeds = parts[0].Replace("seeds: ", string.Empty).Split(" ").Select(long.Parse).ToArray();

        List<MapRange> GetRange(int index, string header)
        {
            var range = ParseMapRanges(parts![index], header).ToList();
            range.Sort();
            return range;
        }

        return new Input(seeds, 
        [
            GetRange(1, "seed-to-soil map:"),
            GetRange(2, "soil-to-fertilizer map:"),
            GetRange(3, "fertilizer-to-water map:"),
            GetRange(4, "water-to-light map:"),
            GetRange(5, "light-to-temperature map:"),
            GetRange(6, "temperature-to-humidity map:"),
            GetRange(7, "humidity-to-location map:")
        ]);
    }

    IEnumerable<MapRange> ParseMapRanges(string input, string header)
    {
        var lines = input.Lines();
        if (lines[0] != header)
        {
            throw new Exception($"Expected header {header} but got {lines[0]}");
        }
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(" ").Select(long.Parse).ToArray();
            yield return new MapRange(parts[0], parts[1], parts[2]);
        }
    }

    internal record class MapRange(long Destination, long Source, long Length) : IComparable<MapRange>
    {
        public long MaxStart => Source + Length;

        public int CompareTo(MapRange? other)
        {
            if (other is null)
            {
                return 1;
            }

            var diff = this.Source - other.Source;
            return diff switch
            {
                > 0 => 1,
                0 => 0,
                < 0 => -1,
            };
        }

        internal bool TryGetDestination(long source, [NotNullWhen(true)] out long? destination)
        {
            if (source < Source || source > Source + Length)
            {
                destination = null;
                return false;
            }

            destination = Destination + (source - Source);
            return true;
        }
    }

    record Input(long[] Seeds, List<List<MapRange>> Maps);

    internal class TreeNode(long Start, long End, int Depth = 0)
    {
        public long Start { get; } = Start;

        public long End { get; } = End;

        public int Depth { get; } = Depth;

        public List<TreeNode> Children = [];

        public void Populate(List<List<MapRange>> maps)
        {
            if (this.Depth >= maps.Count)
            {
                return;
            }

            var mapped = new List<(long Start, long End)>();
            var unmapped = new List<(long Start, long End)>
            {
                (this.Start, this.End)
            };

            foreach (var map in maps[this.Depth])
            {
                var unmappedTemp = new List<(long Start, long End)>();
                foreach (var f in unmapped)
                {
                    (long x, long y) a = (f.Start, Math.Min(f.End, map.Source));
                    (long x, long y) b = (Math.Max(f.Start, map.Source), Math.Min(f.End, map.MaxStart));
                    (long x, long y) c = (Math.Max(f.Start, map.MaxStart), f.End);

                    if (a.x < a.y) unmappedTemp.Add(a);
                    if (b.x < b.y) mapped.Add((b.x - map.Source + map.Destination, b.y - map.Source + map.Destination));
                    if (c.x < c.y) unmappedTemp.Add(c);
                }

                unmapped = unmappedTemp;
            }

            foreach (var (start, end) in mapped)
            {
                var node = new TreeNode(start, end, Depth + 1);
                node.Populate(maps);
                this.Children.Add(node);
            }

            foreach (var (start, end) in unmapped)
            {
                var node = new TreeNode(start, end, Depth + 1);
                node.Populate(maps);
                this.Children.Add(node);
            }
        }

        public IEnumerable<TreeNode> Flatten()
        {
            yield return this;

            foreach (var child in this.Children.SelectMany(c => c.Flatten()))
            {
                yield return child;
            }
        }
    }
}
