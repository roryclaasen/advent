namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;

public class TravellingSalesmanByRoadDistance
{
    public record Road(string From, string To, float Distance);

    public static List<Road> CityRouteToRoadRoute(List<string> route, List<Road> roads)
    {
        var result = new List<Road>();
        for (int i = 0; i < route.Count - 1; i++)
        {
            var road = roads.FirstOrDefault(r => r.From == route[i] && r.To == route[i + 1]);
            if (road is null)
            {
                throw new ArgumentException($"No road found between {route[i]} and {route[i + 1]}");
            }
            result.Add(road);
        }

        return result;
    }

    public static List<string> SolveLongest(List<Road> roads, string start, string end)
    {
        var cities = roads.SelectMany(r => new[] { r.From, r.To }).Distinct().ToList();
        var permutations = GetPermutations(cities, start, end);
        var longestPath = new List<string>();
        var longestDistance = float.MinValue;

        foreach (var permutation in permutations)
        {
            var distance = 0f;
            for (int i = 0; i < permutation.Count - 1; i++)
            {
                var road = roads.FirstOrDefault(r => r.From == permutation[i] && r.To == permutation[i + 1]);
                if (road is null)
                {
                    throw new ArgumentException($"No road found between {permutation[i]} and {permutation[i + 1]}");
                }
                distance += road.Distance;
            }
            if (distance > longestDistance)
            {
                longestDistance = distance;
                longestPath = permutation;
            }
        }

        return longestPath;
    }

    public static List<string> SolveShortest(List<Road> roads, string start, string end)
    {
        var cities = roads.SelectMany(r => new[] { r.From, r.To }).Distinct().ToList();
        var permutations = GetPermutations(cities, start, end);
        var shortestPath = new List<string>();
        var shortestDistance = float.MaxValue;

        foreach (var permutation in permutations)
        {
            var distance = 0f;
            for (int i = 0; i < permutation.Count - 1; i++)
            {
                var road = roads.FirstOrDefault(r => r.From == permutation[i] && r.To == permutation[i + 1]);
                if (road is null)
                {
                    throw new ArgumentException($"No road found between {permutation[i]} and {permutation[i + 1]}");
                }
                distance += road.Distance;
            }
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                shortestPath = permutation;
            }
        }

        return shortestPath;
    }

    private static List<List<string>> GetPermutations(List<string> cities, string start, string end)
    {
        var permutations = new List<List<string>>();
        var visited = new HashSet<string> { start };
        Permute(cities, visited, new List<string> { start }, permutations, end);
        return permutations;
    }

    private static void Permute(List<string> cities, HashSet<string> visited, List<string> permutation, List<List<string>> permutations, string end)
    {
        if (permutation.Count == cities.Count && permutation.Last() == end)
        {
            permutations.Add(permutation);
            return;
        }

        foreach (var city in cities)
        {
            if (!visited.Contains(city))
            {
                visited.Add(city);
                var newPermutation = new List<string>(permutation) { city };
                Permute(cities, visited, newPermutation, permutations, end);
                visited.Remove(city);
            }
        }
    }
}