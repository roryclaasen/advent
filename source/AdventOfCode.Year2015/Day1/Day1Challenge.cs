namespace AdventOfCode.Year2015
{
    using AdventOfCode.Infrastructure;
    using System.Threading.Tasks;

    public class Day1Challenge : IChallenge
    {
        public Task<string> SolvePart1(string input)
        {
            var floor = 0;
            foreach (var c in input)
            {
                if (c == '(')
                {
                    floor++;
                }
                else if (c == ')')
                {
                    floor--;
                }
            }

            return this.Answer(floor);
        }

        public Task<string> SolvePart2(string input)
        {
            var floor = 0;
            var position = 0;
            foreach (var c in input)
            {
                position++;

                if (c == '(')
                {
                    floor++;
                }
                else if (c == ')')
                {
                    floor--;
                }

                if (floor < 0)
                {
                    break;
                }
            }

            return this.Answer(position);
        }
    }
}