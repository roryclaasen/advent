namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public interface IRunner
    {
        int Year { get; }

        int Day { get; }

        Task<string> SolvePart1();

        Task<string> SolvePart2();
    }
}
