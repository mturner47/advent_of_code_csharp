using Helpers.Helpers;

namespace AdventOfCode.Year2015
{
    internal class Day24 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var packages = ParsePackages(lines);
            var totalWeight = packages.Sum();
            var sectionWeight = totalWeight / 3;

            var qe = 0d;
            for (var i = 5; i < 8; i++)
            {
                var bestCombination = GetPossibleCombinationsOfSize(packages, i)
                    .Where(c => c.Sum() == sectionWeight)
                    .OrderBy(QuantumEntanglement)
                    .FirstOrDefault();
                if (bestCombination != null)
                {
                    qe = QuantumEntanglement(bestCombination);
                    break;
                }
            }

            var expectedResult = 11266889531;
            var result = qe;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var packages = ParsePackages(lines);
            var totalWeight = packages.Sum();
            var sectionWeight = totalWeight / 4;

            var qe = 0d;
            for (var i = 2; i < 8; i++)
            {
                var bestCombination = GetPossibleCombinationsOfSize(packages, i)
                    .Where(c => c.Sum() == sectionWeight)
                    .OrderBy(QuantumEntanglement)
                    .FirstOrDefault();
                if (bestCombination != null)
                {
                    qe = QuantumEntanglement(bestCombination);
                    break;
                }
            }

            var expectedResult = 77387711;
            var result = qe;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<int> ParsePackages(IList<string> lines)
        {
            return lines.Select(l => int.Parse(l)).ToList();
        }

        private static double QuantumEntanglement(List<int> packages)
        {
            return packages.Aggregate(1d, (seed, x) => seed * x);
        }

        private static List<List<int>> GetPossibleCombinationsOfSize(List<int> items, int size)
        {
            var results = new List<List<int>>();
            if (size == 1) return items.Select(i => new List<int> { i }).ToList();
            if (size == 0) return new List<List<int>>();
            for (var i = 0; i <= items.Count - size; i++)
            {
                var additionalItems = GetPossibleCombinationsOfSize(items.Skip(i + 1).ToList(), size - 1);
                results.AddRange(additionalItems.Select(ai =>
                {
                    ai.Insert(0, items[i]);
                    return ai;
                }));
            }
            return results;
        }
    }
}
