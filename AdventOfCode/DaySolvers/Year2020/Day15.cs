namespace AdventOfCode.Year2020
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var spokenNumbers = lines[0].Split(',').Select(int.Parse).ToList();
            var target = 2020;
            for (var i = spokenNumbers.Count; i < target; i++)
            {
                var priorNumber = spokenNumbers[i - 1];
                var matchingNumbers = spokenNumbers.Select((n, i) => (n, i)).Where(a => a.n == priorNumber && a.i != i - 1);
                var matchingNumber = matchingNumbers.LastOrDefault();
                if (matchingNumber == default) spokenNumbers.Add(0);
                else spokenNumbers.Add(i - 1 - matchingNumber.i);
            }

            var expectedResult = 1015;
            var result = spokenNumbers.Last();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var spokenNumbers = lines[0].Split(',').Select(int.Parse).ToList();
            var lastSeen = new Dictionary<int, int>();
            var latestNumber = 0;
            for (var i = 0; i < spokenNumbers.Count - 1; i++)
            {
                lastSeen[spokenNumbers[i]] = i;
            }
            var target = 30_000_000;
            latestNumber = spokenNumbers.Last();

            for (var i = spokenNumbers.Count; i < target; i++)
            {
                if (!lastSeen.ContainsKey(latestNumber))
                {
                    lastSeen[latestNumber] = i - 1;
                    latestNumber = 0;
                }
                else
                {
                    var diff = (i - 1) - lastSeen[latestNumber];
                    lastSeen[latestNumber] = i - 1;
                    latestNumber = diff;
                }
            }

            var expectedResult = 1015;
            var result = latestNumber;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
