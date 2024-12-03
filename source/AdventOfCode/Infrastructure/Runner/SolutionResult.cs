namespace AdventOfCode.Infrastructure;

internal record struct SolutionResult(ProblemPartResult Part1, ProblemPartResult Part2)
{
    public readonly bool HasError => this.Part1.IsError || this.Part2.IsError;
}
