// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2015;

using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using AdventOfCode.Problem;

[Problem(2015, 12, "JSAbacusFramework.io")]
public partial class Day12Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var numbers = NumberRegex()
            .Matches(input)
            .Select(m => int.Parse(m.Value));
        return numbers.Sum();
    }

    public object? PartTwo(string input)
    {
        var jsonObject = JsonNode.Parse(input) ?? throw new JsonException("Failed to parse input");
        return this.CountNumbersWithoutRed(jsonObject);
    }

    [GeneratedRegex("-?\\d+")]
    private static partial Regex NumberRegex();

    private int CountNumbersWithoutRed(JsonNode? node) => node switch
    {
        JsonObject jsonObject when !this.HasRed(jsonObject) => jsonObject.Select(j => j.Value).Sum(this.CountNumbersWithoutRed),
        JsonValue jsonValue when jsonValue.TryGetValue<int>(out var value) => value,
        JsonArray jsonArray => jsonArray.Sum(this.CountNumbersWithoutRed),
        _ => 0
    };

    private bool HasRed(JsonNode? node) => node switch
    {
        JsonValue jsonValue => jsonValue.TryGetValue<string>(out var value) && value == "red",
        JsonObject jsonObject => jsonObject.Select(j => j.Value).OfType<JsonValue>().Any(this.HasRed),
        _ => false
    };
}
