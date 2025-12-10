// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Year2025;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Problem;
using AdventOfCode.Shared;

[Problem(2025, 10, "Factory")]
public partial class Day10Solution : IProblemSolver
{
    public object? PartOne(string input) => ParseInput(input).Sum(FindMinimumPresses);

    public object? PartTwo(string input) => ParseInput(input).Sum(FindMinimumJoltagePresses);

    private static int FindMinimumPresses(Machine machine)
    {
        var targetMask = 0;
        for (var i = 0; i < machine.IndicatorLights.Length; i++)
        {
            if (machine.IndicatorLights[i])
            {
                targetMask |= 1 << i;
            }
        }

        var numButtons = machine.ButtonWiringSchematics.Length;
        var buttonMasks = new int[numButtons];
        for (var b = 0; b < numButtons; b++)
        {
            var mask = 0;
            foreach (var lightIndex in machine.ButtonWiringSchematics[b])
            {
                mask |= 1 << lightIndex;
            }

            buttonMasks[b] = mask;
        }

        for (var pressCount = 0; pressCount <= numButtons; pressCount++)
        {
            foreach (var subset in MathUtils.GetSubsetsOfSize(numButtons, pressCount))
            {
                var resultMask = 0;
                foreach (var buttonIndex in subset)
                {
                    resultMask ^= buttonMasks[buttonIndex];
                }

                if (resultMask == targetMask)
                {
                    return pressCount;
                }
            }
        }

        return -1;
    }

    private static long FindMinimumJoltagePresses(Machine machine)
    {
        var numCounters = machine.JoltageRequirement.Length;
        var numButtons = machine.ButtonWiringSchematics.Length;

        var matrix = new long[numCounters, numButtons + 1];
        for (var c = 0; c < numCounters; c++)
        {
            matrix[c, numButtons] = machine.JoltageRequirement[c];
        }

        for (var b = 0; b < numButtons; b++)
        {
            foreach (var counter in machine.ButtonWiringSchematics[b])
            {
                if (counter < numCounters)
                {
                    matrix[counter, b] = 1;
                }
            }
        }

        var pivotCols = new List<int>();
        var pivotRow = 0;

        for (var col = 0; col < numButtons && pivotRow < numCounters; col++)
        {
            var foundPivot = -1;
            for (var row = pivotRow; row < numCounters; row++)
            {
                if (matrix[row, col] != 0)
                {
                    foundPivot = row;
                    break;
                }
            }

            if (foundPivot == -1)
            {
                continue;
            }

            if (foundPivot != pivotRow)
            {
                for (var j = 0; j <= numButtons; j++)
                {
                    (matrix[pivotRow, j], matrix[foundPivot, j]) = (matrix[foundPivot, j], matrix[pivotRow, j]);
                }
            }

            pivotCols.Add(col);
            for (var row = 0; row < numCounters; row++)
            {
                if (row != pivotRow && matrix[row, col] != 0)
                {
                    var factor = matrix[row, col];
                    var pivotVal = matrix[pivotRow, col];
                    for (var j = 0; j <= numButtons; j++)
                    {
                        matrix[row, j] = (matrix[row, j] * pivotVal) - (factor * matrix[pivotRow, j]);
                    }
                }
            }

            pivotRow++;
        }

        var freeVars = new List<int>();
        for (var b = 0; b < numButtons; b++)
        {
            if (!pivotCols.Contains(b))
            {
                freeVars.Add(b);
            }
        }

        if (freeVars.Count == 0)
        {
            long sum = 0;
            for (var i = 0; i < pivotCols.Count; i++)
            {
                var value = matrix[i, numButtons] / matrix[i, pivotCols[i]];
                sum += value;
            }

            return sum;
        }

        return SearchMinimum(matrix, pivotCols, freeVars, numButtons);
    }

    private static long SearchMinimum(long[,] matrix, List<int> pivotCols, List<int> freeVars, int numButtons)
    {
        var minSum = long.MaxValue;
        var numFree = freeVars.Count;

        var maxFreeValue = 0L;
        for (var i = 0; i < pivotCols.Count; i++)
        {
            maxFreeValue = Math.Max(maxFreeValue, Math.Abs(matrix[i, numButtons] / matrix[i, pivotCols[i]]));
        }

        maxFreeValue = Math.Min(maxFreeValue + 10, 300);

        var freeValues = new long[numFree];
        var indices = new int[numFree];

        while (true)
        {
            for (var i = 0; i < numFree; i++)
            {
                freeValues[i] = indices[i];
            }

            var solution = new long[numButtons];
            var valid = true;
            long currentSum = 0;

            for (var i = 0; i < numFree; i++)
            {
                solution[freeVars[i]] = freeValues[i];
                currentSum += freeValues[i];
            }

            if (currentSum >= minSum)
            {
                valid = false;
            }

            if (valid)
            {
                for (var i = pivotCols.Count - 1; i >= 0 && valid; i--)
                {
                    var col = pivotCols[i];
                    var value = matrix[i, numButtons];

                    for (var j = col + 1; j < numButtons; j++)
                    {
                        value -= matrix[i, j] * solution[j];
                    }

                    var pivotVal = matrix[i, col];
                    if (value % pivotVal != 0)
                    {
                        valid = false;
                    }
                    else
                    {
                        solution[col] = value / pivotVal;
                        if (solution[col] < 0)
                        {
                            valid = false;
                        }
                        else
                        {
                            currentSum += solution[col];
                            if (currentSum >= minSum)
                            {
                                valid = false;
                            }
                        }
                    }
                }
            }

            if (valid)
            {
                minSum = currentSum;
            }

            var carry = true;
            for (var i = 0; i < numFree && carry; i++)
            {
                indices[i]++;
                if (indices[i] <= maxFreeValue)
                {
                    carry = false;
                }
                else
                {
                    indices[i] = 0;
                }
            }

            if (carry)
            {
                break;
            }
        }

        return minSum;
    }

    private static IEnumerable<Machine> ParseInput(string input)
    {
        foreach (var line in input.Lines())
        {
            var lineSpan = line.AsSpan();

            var lights = ImmutableArray.CreateBuilder<bool>();
            var buttons = ImmutableArray.CreateBuilder<ImmutableArray<int>>();
            var joltage = ImmutableArray.CreateBuilder<int>();

            var lightsEnd = lineSpan.IndexOf(']');
            for (var l = 1; l < lightsEnd; l++)
            {
                lights.Add(lineSpan[l] == '#');
            }

            var buttonStart = Offset(lineSpan[lightsEnd..].IndexOf('('), lightsEnd + 1);
            while (buttonStart != -1)
            {
                var buttonEnd = lineSpan[buttonStart..].IndexOf(')') + buttonStart;
                var buttonInner = ImmutableArray.CreateBuilder<int>();

                var buttonSpan = lineSpan[buttonStart..buttonEnd];
                foreach (var bits in buttonSpan.Split(','))
                {
                    buttonInner.Add(int.Parse(buttonSpan[bits]));
                }

                buttons.Add(buttonInner.ToImmutable());

                buttonStart = Offset(lineSpan[buttonEnd..].IndexOf('('), buttonEnd + 1);
            }

            var joltageStart = lineSpan.IndexOf('{') + 1;
            var joltageEnd = lineSpan.IndexOf('}');

            var joltageSpan = lineSpan[joltageStart..joltageEnd];
            foreach (var bits in joltageSpan.Split(','))
            {
                joltage.Add(int.Parse(joltageSpan[bits]));
            }

            yield return new Machine(lights.ToImmutable(), buttons.ToImmutable(), joltage.ToImmutable());
        }

        static int Offset(int start, int offset) => start == -1 ? -1 : start + offset;
    }

    private record struct Machine(
        ImmutableArray<bool> IndicatorLights,
        ImmutableArray<ImmutableArray<int>> ButtonWiringSchematics,
        ImmutableArray<int> JoltageRequirement);
}
