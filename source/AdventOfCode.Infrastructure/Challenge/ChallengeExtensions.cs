namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public static class ChallengeExtensions
    {
        public static Task<string> Answer(this IChallenge challenge, object answer) => challenge.Answer(Task.FromResult(answer));

        public static async Task<string> Answer(this IChallenge _, Task<object> answerTask)
        {
            var answer = await answerTask.ConfigureAwait(false);
            return answer.ToString();
        }
    }
}
