namespace AdventOfCode.Year2016
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var ranges = lines.Select(ParseLine).OrderBy(x => x.lower).ToList();
            var currentLowest = 0L;
            while (currentLowest >= ranges[0].lower)
            {
                currentLowest = ranges[0].upper + 1;
                ranges = ranges.Where(r => r.upper >= currentLowest).ToList();
            }

            var expectedResult = 32259706;
            var result = currentLowest;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var ranges = lines.Select(ParseLine).OrderBy(x => x.lower).ToList();
            var currentTotal = 0L;
            var currentCheck = 0L;
            var maxValue = 4294967295L;

            while (currentCheck < maxValue)
            {
                if (ranges.Count == 0)
                {
                    currentTotal += maxValue - currentCheck;
                    currentCheck = maxValue;
                }
                else
                {
                    if (currentCheck < ranges[0].lower)
                    {
                        currentTotal += ranges[0].lower - currentCheck;
                    }

                    currentCheck = ranges[0].upper + 1;
                    ranges = ranges.Where(r => r.upper >= currentCheck).ToList();
                }
            }

            var expectedResult = -1;
            var result = currentTotal;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (long lower, long upper) ParseLine(string line)
        {
            var parts = line.Split("-");
            return (long.Parse(parts[0]), long.Parse(parts[1]));
        }
    }
}
