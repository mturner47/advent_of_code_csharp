namespace AdventOfCode.Year2020
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var adapters = lines.Select(int.Parse).OrderBy(i => i).ToList();

            var oneDiffCount = 0;
            var threeDiffCount = 0;
            adapters.Insert(0, 0);
            adapters.Add(adapters.Max() + 3);
            for (var i = 1; i < adapters.Count; i++)
            {
                if (adapters[i] - adapters[i - 1] == 1) oneDiffCount++;
                else threeDiffCount++;
            }

            var expectedResult = 1690;
            var result = oneDiffCount*threeDiffCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var adapters = lines.Select(int.Parse).OrderBy(i => i).ToList();
            adapters.Insert(0, 0);
            adapters.Add(adapters.Max() + 3);

            var knownArrangements = new Dictionary<int, long> { { 0, 1 } };

            for (var i = 1; i < adapters.Count; i++)
            {
                var val = adapters[i];
                var count = 0L;
                for (var j = 1; j <= 3; j++)
                {
                    if (i - j < 0) break;
                    if (adapters[i - j] + 3 < val) break;
                    count += knownArrangements[i - j];
                }
                knownArrangements[i] = count;
            }

            var expectedResult = 5289227976704;
            var result = knownArrangements[adapters.Count - 1];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
