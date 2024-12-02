namespace AdventOfCode.Shared;

public interface ISolverWithDetails : ISolver
{
    public int Year { get; }

    public int Day { get; }

    public string Name { get; }
}
