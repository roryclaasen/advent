// ------------------------------------------------------------------------------
// <copyright file="ChallengeExtensions.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure
{
    using System.Threading.Tasks;

    public static class ChallengeExtensions
    {
        public static Task<string> Answer(this IChallenge challenge, object answer) => challenge.Answer(Task.FromResult(answer));

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public static async Task<string> Answer(this IChallenge _, Task<object> answerTask)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            var answer = await answerTask.ConfigureAwait(false);
            return answer?.ToString() ?? string.Empty;
        }
    }
}
