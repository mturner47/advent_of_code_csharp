using Helpers.Helpers;

namespace AdventOfCode.Year2015
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 1304;
            var result = MathHelpers.GetCombinations(lines.Select(int.Parse).ToList()).Count(c => c.Sum() == 150);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var validCombinations = MathHelpers.GetCombinations(lines.Select(int.Parse).ToList()).Where(c => c.Sum() == 150).ToList();
            var minCount = validCombinations.OrderBy(c => c.Count).First().Count;

            var expectedResult = -1;
            var result = validCombinations.Count(c => c.Count == minCount);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
