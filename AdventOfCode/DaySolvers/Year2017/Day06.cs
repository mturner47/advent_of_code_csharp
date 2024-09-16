namespace AdventOfCode.Year2017
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var banks = lines[0].Split('\t').Select(int.Parse).ToList();
            var seenChecksums = new List<string> { CheckSum(banks) };
            var count = 0;

            while (true)
            {
                var maxAmount = banks.Max();
                var maxIndex = banks.IndexOf(maxAmount);
                banks[maxIndex] = 0;

                for (var i = 1; i <= maxAmount; i++)
                {
                    var index = (maxIndex + i) % banks.Count;
                    banks[index]++;
                }
                count++;
                var newChecksum = CheckSum(banks);
                if (seenChecksums.Contains(newChecksum))
                {
                    break;
                }
                seenChecksums.Add(newChecksum);
            }

            var expectedResult = 11137;
            var result = count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var banks = lines[0].Split('\t').Select(int.Parse).ToList();
            var seenChecksums = new List<string> { CheckSum(banks) };
            var loopSize = 0;

            while (true)
            {
                var maxAmount = banks.Max();
                var maxIndex = banks.IndexOf(maxAmount);
                banks[maxIndex] = 0;

                for (var i = 1; i <= maxAmount; i++)
                {
                    var index = (maxIndex + i) % banks.Count;
                    banks[index]++;
                }

                var newChecksum = CheckSum(banks);
                if (seenChecksums.Contains(newChecksum))
                {
                    var indexOfSharedChecksum = seenChecksums.IndexOf(newChecksum);
                    var newIndex = seenChecksums.Count;
                    loopSize = newIndex - indexOfSharedChecksum;
                    break;
                }
                seenChecksums.Add(newChecksum);
            }

            var expectedResult = 1037;
            var result = loopSize;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string CheckSum(List<int> values)
        {
            return string.Join(",", values);
        }
    }
}
