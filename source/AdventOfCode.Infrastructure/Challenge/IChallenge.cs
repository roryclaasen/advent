namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public interface IChallenge
    {
        public Task<string> SolvePart1(string input);

        public Task<string> SolvePart2(string input);
    }
}
