namespace AdventOfCode.Year2015.Day1
{
    using AdventOfCode.Infrastructure;
    using System;
    using System.Threading.Tasks;

    public class Day1Runner : Runner<Day1Challenge>
    {
        public override int Year => 2015;

        public override int Day => 1;

        public override Task<string> SolvePart1()
        {
            var input = Properties.Resources.Day1Part1;
            return this.Challenge.Value.SolvePart1(input);
        }

        public override Task<string> SolvePart2()
        {
            throw new NotImplementedException();
        }
    }
}
