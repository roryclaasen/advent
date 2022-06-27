namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public abstract class Challenge
    {
        public abstract Task<string> SolvePart1(string input);

        public abstract Task<string> SolvePart2(string input);
    }
}
