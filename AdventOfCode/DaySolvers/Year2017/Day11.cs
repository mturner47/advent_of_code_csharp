using System.Linq;

namespace AdventOfCode.Year2017
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var steps = lines[0].Split(",").ToList();
            var expectedResult = 747;
            var result = GetDistance(steps);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var maxDistance = 0;
            var steps = lines[0].Split(",").ToList();
            var currentSteps = new List<string>();

            for (var i = 0; i < steps.Count; i++)
            {
                currentSteps.Add(steps[i]);
                var distance = GetDistance(currentSteps);
                if (maxDistance < distance) maxDistance = distance;
            }

            var expectedResult = 1544;
            var result = maxDistance;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int GetDistance(List<string> steps)
        {
            var groups = steps.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            if (groups.Count < 6) return 0;
            var oppositePairs = new List<(string, string)> { ("s", "n"), ("se", "nw"), ("ne", "sw") };
            foreach (var (a, b) in oppositePairs)
            {
                var sharedAmount = Math.Min(groups[a], groups[b]);
                groups[a] -= sharedAmount;
                groups[b] -= sharedAmount;
            }

            var reductions = new List<(string, string, string)>
            {
                ("nw", "ne", "n"),
                ("n", "se", "ne"),
                ("ne", "s", "se"),
                ("se", "sw", "s"),
                ("s", "nw", "sw"),
                ("sw", "n", "nw"),
            };

            while (groups.Count(g => g.Value > 0) > 2)
            {
                foreach (var (a, b, c) in reductions)
                {
                    var sharedAmount = Math.Min(groups[a], groups[b]);
                    groups[a] -= sharedAmount;
                    groups[b] -= sharedAmount;
                    groups[c] += sharedAmount;
                }
            }

            return groups.Values.Sum();
        }
    }
}
