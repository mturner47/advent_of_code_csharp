namespace AdventOfCode.Year2020
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 6590;
            var result = Parse(lines).Sum(g => string.Join("", g).Distinct().Count());
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 3288;
            var result = Parse(lines).Sum(Count);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<List<string>> Parse(IList<string> lines)
        {
            return string.Join("\n", lines).Split("\n\n").Select(l => l.Split("\n").ToList()).ToList();
        }

        private static int Count(List<string> group)
        {
            var intersection = group[0];
            foreach (var person in group)
            {
                intersection = new string(intersection.Intersect(person).ToArray());
            }
            return intersection.Length;
        }
    }
}
