using AdventOfCode.DaySolvers.Year2017;

namespace AdventOfCode.Year2017
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var size = 256;
            var lengths = lines[0].Split(',').Select(int.Parse).ToList();
            var values = Enumerable.Range(0, size).ToList();
            var currentPosition = 0;
            var skipSize = 0;
            for (var i = 0; i < lengths.Count; i++)
            {
                var length = lengths[i];
                var start = currentPosition;
                var end = (currentPosition + length - 1) % size;
                for (var j = 1; j <= length/2; j++)
                {
                    (values[start], values[end]) = (values[end], values[start]);
                    start++;
                    end--;
                    if (end < 0) end = size - 1;
                    if (start == size) start = 0;
                }
                currentPosition = (currentPosition + length + skipSize)%size;
                skipSize++;
            }

            var expectedResult = 62238;
            var result = values[0]*values[1];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = "2B0C9CC0449507A0DB3BABD57AD9E8D8";
            var result = Shared2017.KnotHash(lines[0]);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
