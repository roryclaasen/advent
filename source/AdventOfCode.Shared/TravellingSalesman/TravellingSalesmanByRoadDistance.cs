// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

using System;
using System.Collections.Generic;
using System.Linq;

public class TravellingSalesmanByRoadDistance
{
    public static List<Road<T>> CityRouteToRoadRoute<T>(List<T> route, List<Road<T>> roads) where T : class
    {
        var result = new List<Road<T>>();
        for (int i = 0; i < route.Count - 1; i++)
        {
            var road = roads.FirstOrDefault(r => r.From.Equals(route[i]) && r.To.Equals(route[i + 1]))
                ?? throw new ArgumentException($"No road found between {route[i]} and {route[i + 1]}");

            result.Add(road);
        }

        return result;
    }

    public static List<T> SolveLongest<T>(List<Road<T>> roads, T start, T end) where T : class
    {
        var cities = roads.SelectMany(r => new[] { r.From, r.To }).Distinct().ToList();
        var longestPath = new List<T>();
        var longestDistance = float.MinValue;

        foreach (var permutation in cities.Permutations(start, end))
        {
            var distance = 0f;
            for (int i = 0; i < permutation.Count - 1; i++)
            {
                var road = roads.FirstOrDefault(r => r.From.Equals(permutation[i]) && r.To.Equals(permutation[i + 1]))
                    ?? throw new ArgumentException($"No road found between {permutation[i]} and {permutation[i + 1]}");

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

    public static List<T> SolveShortest<T>(List<Road<T>> roads, T start, T end) where T : class
    {
        var cities = roads.SelectMany(r => new[] { r.From, r.To }).Distinct().ToList();
        var shortestPath = new List<T>();
        var shortestDistance = float.MaxValue;

        foreach (var permutation in cities.Permutations(start, end))
        {
            var distance = 0f;
            for (int i = 0; i < permutation.Count - 1; i++)
            {
                var road = roads.FirstOrDefault(r => r.From.Equals(permutation[i]) && r.To.Equals(permutation[i + 1]))
                    ?? throw new ArgumentException($"No road found between {permutation[i]} and {permutation[i + 1]}");

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

    public record Road<T>(T From, T To, float Distance) where T : class;
}
