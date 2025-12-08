// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2015, 9, "All in a Single Night")]
public partial class Day9Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var roads = ParseInput(input).ToList();
        var routes = FindAllRoutes(roads, TravellingSalesmanByRoadDistance.SolveShortest)
            .Select(route => route.Sum(road => road.Distance));
        return routes.Min();
    }

    public object? PartTwo(string input)
    {
        var roads = ParseInput(input).ToList();
        var routes = FindAllRoutes(roads, TravellingSalesmanByRoadDistance.SolveLongest)
            .Select(route => route.Sum(road => road.Distance));
        return routes.Max();
    }

    private static IEnumerable<List<TravellingSalesmanByRoadDistance.Road<string>>> FindAllRoutes(
        List<TravellingSalesmanByRoadDistance.Road<string>> roads,
        Func<List<TravellingSalesmanByRoadDistance.Road<string>>, string, string, List<string>> algorithm)
    {
        var cities = roads.SelectMany(r => new[] { r.From, r.To }).Distinct();
        var routes = cities
            .SelectMany(from => cities
                .Where(city => city != from)
                .Select(to => algorithm(roads, from, to))
                .Select(route => TravellingSalesmanByRoadDistance.CityRouteToRoadRoute(route, roads)));
        return routes;
    }

    private static IEnumerable<TravellingSalesmanByRoadDistance.Road<string>> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var match = InputLineRegex().Match(line);
            if (!match.Success)
            {
                throw new ValidationException($"Input line '{line}' did not match expected format.");
            }

            var from = match.Groups["From"].Value;
            var to = match.Groups["To"].Value;
            var distance = int.Parse(match.Groups["Distance"].Value);
            yield return new TravellingSalesmanByRoadDistance.Road<string>(from, to, distance);
            yield return new TravellingSalesmanByRoadDistance.Road<string>(to, from, distance);
        }
    }

    [GeneratedRegex("^(?<From>\\w+) to (?<To>\\w+) = (?<Distance>\\d+)$")]
    private static partial Regex InputLineRegex();
}
