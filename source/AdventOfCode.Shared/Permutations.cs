namespace AdventOfCode.Shared
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Permutations
    {
        public static List<List<T>> GetPermutations<T>(IEnumerable<T> values, T? start = null, T? end = null) where T : class
        {
            var permutations = new List<List<T>>();
            var visited = new HashSet<T>();
            var permutation = new List<T>();
            if (start is not null)
            {
                visited.Add(start);
                permutation.Add(start);
            }
            Permute(values, visited, permutation, permutations, end);
            return permutations;
        }

        private static void Permute<T>(IEnumerable<T> values, HashSet<T> visited, List<T> permutation, List<List<T>> permutations, T? end = null) where T : class
        {
            if (permutation.Count == values.Count() && (end is null || permutation.Last()!.Equals(end)))
            {
                permutations.Add(permutation);
                return;
            }

            foreach (var value in values)
            {
                if (!visited.Contains(value))
                {
                    visited.Add(value);
                    var newPermutation = new List<T>(permutation) { value };
                    Permute(values, visited, newPermutation, permutations, end);
                    visited.Remove(value);
                }
            }
        }
    }
}
