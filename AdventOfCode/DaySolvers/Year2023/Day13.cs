using Helpers.Extensions;

namespace AdventOfCode.Year2023
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var patterns = ParseLines(lines).ToList();
            return patterns.Sum(p => GetPatternValueFinal(p, 0));
        }

        public object HardSolution(IList<string> lines)
        {
            var patterns = ParseLines(lines).ToList();
            return patterns.Sum(p => GetPatternValueFinal(p, 1));
        }

        private static IEnumerable<List<string>> ParseLines(IList<string> lines)
        {
            var patternGroups = string.Join(Environment.NewLine, lines).Split(Environment.NewLine + Environment.NewLine).ToList();
            foreach (var patternGroup in patternGroups)
            {
                yield return patternGroup.Split(Environment.NewLine).ToList();
            }
        }

        private static int GetPatternValueFinal(List<string> pattern, int expectedNumWrong)
        {
            var val = GetPatternValue(pattern, expectedNumWrong);
            if (val != 0) return val * 100;

            var transposedPattern = pattern.Transpose();
            return GetPatternValue(transposedPattern, expectedNumWrong);
        }

        private static int GetPatternValue(List<string> pattern, int expectedNumWrong)
        {
            for (var index = 1; index < pattern.Count; index++)
            {
                var numWrong = 0;
                var maxDistance = Math.Min(index - 0, pattern.Count - index);
                for (var i = 1; i < maxDistance + 1; i++)
                {
                    numWrong += DifferenceCount(pattern[index - i], pattern[index + (i - 1)]);
                    if (numWrong > expectedNumWrong) break;
                }

                if (numWrong == expectedNumWrong)
                {
                    return index;
                }
            }
            return 0;
        }

        private static int DifferenceCount(string s1, string s2)
        {
            var count = Math.Abs(s1.Length - s2.Length);
            for (var i = 0; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[i] != s2[i]) count++;
            }
            return count;
        }
    }
}
