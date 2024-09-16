namespace AdventOfCode.Year2019
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var parts = lines[0].Split("-");
            var minAmount = int.Parse(parts[0]);
            var maxAmount = int.Parse(parts[1]);

            var matchCount = 0;
            for (var i = minAmount; i <= maxAmount; i++)
            {
                if (MeetsEasyCriteria(i)) matchCount++;
            }
            return matchCount;
        }

        public object HardSolution(IList<string> lines)
        {
            var parts = lines[0].Split("-");
            var minAmount = int.Parse(parts[0]);
            var maxAmount = int.Parse(parts[1]);

            var matchCount = 0;
            for (var i = minAmount; i <= maxAmount; i++)
            {
                if (MeetsHardCriteria(i)) matchCount++;
            }
            return matchCount;
        }

        private static bool MeetsEasyCriteria(int input)
        {
            var digits = $"{input}".ToList();
            var pairFound = false;
            for (var i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] > digits[i + 1]) return false;
                if (digits[i] == digits[i + 1]) pairFound = true;
            }
            return pairFound;
        }

        private static bool MeetsHardCriteria(int input)
        {
            var digits = $"{input}".ToList();
            var pairFound = false;
            for (var i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] > digits[i + 1]) return false;
                if (digits[i] == digits[i + 1])
                {
                    if (i + 2 >= digits.Count || digits[i + 2] != digits[i])
                    {
                        pairFound = true;
                    }
                    else
                    {
                        var j = i + 1;
                        while (j < digits.Count && digits[j] == digits[i])
                        {
                            j++;
                        }
                        i = j - 2;
                    }
                }
            }
            return pairFound;
        }
    }
}
