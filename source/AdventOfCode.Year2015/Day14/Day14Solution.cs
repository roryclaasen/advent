namespace AdventOfCode.Year2015;

using AdventOfCode.Problem;
using AdventOfCode.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Problem(2015, 14, "Reindeer Olympics")]
public partial class Day14Solution : IProblemSolver
{
    public int TotalSeconds { get; set; } = 2503;

    public object? PartOne(string input)
        => Parse(input)
            .Select(r => DistanceTraveled(r, this.TotalSeconds))
            .Max();

    public object? PartTwo(string input)
    {
        var reindeer = Parse(input).ToList();
        var scores = new Dictionary<Reindeer, int>();
        for (var i = 1; i <= this.TotalSeconds; i++)
        {
            var distances = reindeer.Select(r => (Reindeer: r, Distance: DistanceTraveled(r, i)));
            var maxDistance = distances.Max(x => x.Distance);
            foreach (var (Reindeer, Distance) in distances.Where(x => x.Distance == maxDistance))
            {
                scores[Reindeer] = scores.GetValueOrDefault(Reindeer) + 1;
            }
        }
        return scores.Values.Max();
    }

    private static int DistanceTraveled(Reindeer reindeer, int seconds)
    {
        var cycleTime = reindeer.FlyTime + reindeer.RestTime;
        var cycles = seconds / cycleTime;
        var remainder = seconds % cycleTime;
        var distance = cycles * reindeer.Speed * reindeer.FlyTime;
        if (remainder > reindeer.FlyTime)
        {
            distance += reindeer.Speed * reindeer.FlyTime;
        }
        else
        {
            distance += reindeer.Speed * remainder;
        }
        return distance;
    }

    private static IEnumerable<Reindeer> Parse(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = ReindeerRegex().Match(line);
            if (match.Success)
            {
                var name = match.Groups["Name"].Value;
                var speed = int.Parse(match.Groups["Speed"].Value);
                var flyTime = int.Parse(match.Groups["FlyTime"].Value);
                var restTime = int.Parse(match.Groups["RestTime"].Value);
                yield return new Reindeer(name, speed, flyTime, restTime);
            }
        }
    }

    private record Reindeer(string Name, int Speed, int FlyTime, int RestTime);

    [GeneratedRegex("(?<Name>\\w+) can fly (?<Speed>\\d+) km/s for (?<FlyTime>\\d+) seconds, but then must rest for (?<RestTime>\\d+) seconds\\.")]
    private static partial Regex ReindeerRegex();
}
