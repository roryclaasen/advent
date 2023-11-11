namespace AdventOfCode.Shared
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Permutations
    {
        public static List<List<T>> GetPermutations<T>(IEnumerable<T> values)
        {
            var permutations = new List<List<T>>();
            Permute(values, new HashSet<T>(), new List<T>(), permutations);
            return permutations;
        }

        public static List<List<T>> GetPermutations<T>(IEnumerable<T> values, T start, T end)
        {
            var permutations = new List<List<T>>();
            var visited = new HashSet<T> { start };
            Permute(values, visited, new List<T> { start }, permutations, end);
            return permutations;
        }

        private static void Permute<T>(IEnumerable<T> values, HashSet<T> visited, List<T> permutation, List<List<T>> permutations, T end)
        {
            if (permutation.Count == values.Count() && permutation.Last()!.Equals(end))
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

        private static void Permute<T>(IEnumerable<T> values, HashSet<T> visited, List<T> permutation, List<List<T>> permutations)
        {
            if (permutation.Count == values.Count())
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
                    Permute(values, visited, newPermutation, permutations);
                    visited.Remove(value);
                }
            }
        }
    }
}
