namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var numberLines = lines.Select(l => l.Select(c => c - '0').ToList()).ToList();
            var expectedResult = 17092;
            var result = numberLines.Sum(nl => GetLargestNumber(nl, 2));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var numberLines = lines.Select(l => l.Select(c => c - '0').ToList()).ToList();
            var expectedResult = 170147128753455d;
            var result = numberLines.Sum(nl => GetLargestNumber(nl, 12));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private double GetLargestNumber(List<int> numbers, int length)
        {
            var best = 0d;
            var minIndex = 0;
            for (var i = 0; i < length; i++)
            {
                var maxNum = -1;
                var maxNumIndex = -1;
                for (var j = minIndex; j <= numbers.Count - (length - i); j++)
                {
                    var jNum = numbers[j];
                    if (jNum > maxNum)
                    {
                        maxNum = jNum;
                        maxNumIndex = j;
                    }
                }
                minIndex = maxNumIndex + 1;
                best = best * 10 + maxNum;
            }
            return best;
        }
    }
}
