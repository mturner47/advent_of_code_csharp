namespace AdventOfCode.Year2020
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var numbers = lines.Select(long.Parse).ToList();
            var index = 25;
            while (index < numbers.Count)
            {
                var current = numbers[index];
                var found = false;
                for (var i = index - 25; i < index - 1; i++)
                {
                    var iNum = numbers[i];
                    for (var j = i + 1; j < index; j++)
                    {
                        if (iNum + numbers[j] == current) found = true;
                    }
                }
                if (!found) break;
                index++;
            }
            var expectedResult = 15690279;
            var result = numbers[index];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var numbers = lines.Select(long.Parse).ToList();
            var size = 25;
            var index = size;
            long target;
            while (index < numbers.Count)
            {
                var current = numbers[index];
                var found = false;
                for (var i = index - size; i < index - 1; i++)
                {
                    var iNum = numbers[i];
                    for (var j = i + 1; j < index; j++)
                    {
                        if (iNum + numbers[j] == current) found = true;
                    }
                }
                if (!found) break;
                index++;
            }
            target = numbers[index];

            var minIndex = 0;
            var maxIndex = 0;
            var val = numbers[0];
            while (val != target)
            {
                if (val < target)
                {
                    maxIndex++;
                    val += numbers[maxIndex];
                }
                else if (val > target)
                {
                    val -= numbers[minIndex];
                    minIndex++;
                }
            }

            var region = numbers.Skip(minIndex).Take(maxIndex - minIndex + 1).ToList();
            var expectedResult = 2174232;
            var result = region.Min() + region.Max();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
