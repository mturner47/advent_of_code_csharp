namespace AdventOfCode.Year2017
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var firewalls = lines.Select(Parse).ToDictionary(x => x.Item1, x => x.Item2);

            var expectedResult = 1580;
            var result = CalculateSeverity(firewalls, 0, true);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var firewalls = lines.Select(Parse).ToDictionary(x => x.Item1, x => x.Item2);
            var expectedResult = -1;
            var result = Enumerable.Range(0, int.MaxValue).Select(i => (t: i, severity: CalculateSeverity(firewalls, i, false))).First(x => x.severity == 0).t;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int, int) Parse(string line)
        {
            var parts = line.Split(": ");
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        private static int CalculateSeverity(Dictionary<int, int> firewalls, int t, bool allowZeroSeverity)
        {
            var maxTime = firewalls.Keys.Max();
            var severity = 0;
            for (var i = 0; i <= maxTime; i++)
            {
                if (firewalls.ContainsKey(i))
                {
                    if ((t + i) % ((firewalls[i] - 1) * 2) == 0) severity += i * firewalls[i] + (allowZeroSeverity ? 0 : 1);
                }
            }
            return severity;
        }
    }
}
