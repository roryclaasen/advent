// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Problem.Extensions;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using AdventOfCode.Problem;

[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this")]
public static class IProblemSolverExtensions
{
    extension(IProblemSolver solver)
    {
        public int Year => solver is IProblemDetails problem ? problem.Year : 0;
        public int Day => solver is IProblemDetails problem ? problem.Day : 0;
        public string Name => solver is IProblemDetails problem ? problem.Name : string.Empty;

        public string? Input => solver is IProblemInput problem ? problem.Input : null;

        public string? Expected1 => solver is IProblemExpectedResultPart1 problem ? problem.Expected1 : null;
        public string? Expected2 => solver is IProblemExpectedResultPart2 problem ? problem.Expected2 : null;

        public string GetDisplayName(bool includeYear = false)
        {
            var sb = new StringBuilder();
            if (includeYear)
            {
                sb.Append($"Year {solver.Year} ");
            }

            sb.Append($"Day {solver.Day}");

            var name = solver.Name;
            if (!string.IsNullOrWhiteSpace(name))
            {
                sb.Append($" - {name}");
            }

            return sb.ToString();
        }
    }

    public static IOrderedEnumerable<IProblemSolver> OrderByYearAndDay(this IEnumerable<IProblemSolver> solvers)
        => solvers.OrderBy(s => s.Year).ThenBy(s => s.Day);

    public static IEnumerable<IGrouping<int, IProblemSolver>> GroupByYear(this IEnumerable<IProblemSolver> solvers)
        => solvers.GroupBy(s => s.Year);
}
