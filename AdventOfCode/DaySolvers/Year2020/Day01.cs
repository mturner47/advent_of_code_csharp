namespace AdventOfCode.Year2020
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var entries = lines.Select(long.Parse).ToList();
            var product = 0L;
            for (var i = 0; i < entries.Count - 1; i++)
            {
                for (var j = i + 1; j < entries.Count; j++)
                {
                    if (entries[i] + entries[j] == 2020) product = entries[i] * entries[j];
                }
            }

            var expectedResult = 1007104;
            var result = product;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var entries = lines.Select(long.Parse).ToList();
            var product = 0L;
            for (var i = 0; i < entries.Count - 2; i++)
            {
                var vi = entries[i];
                for (var j = i + 1; j < entries.Count - 1; j++)
                {
                    var vj = entries[j];
                    for (var k = j + 1; k < entries.Count; k++)
                    {
                        var vk = entries[k];
                        if (vi + vj + vk == 2020) product = vi * vj * vk;
                    }
                }
            }

            var expectedResult = 18847752;
            var result = product;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
