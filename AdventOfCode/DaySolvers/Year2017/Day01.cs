namespace AdventOfCode.Year2017
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var values = lines[0].Select(c => int.Parse(c.ToString())).ToList();
            var sum = 0;
            for (var i = 0; i < values.Count - 1; i++)
            {
                if (i == 0 && values[i] == values[^1]) sum += values[i];
                if (values[i] == values[i + 1]) sum += values[i];
            }
            var expectedResult = 995;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var values = lines[0].Select(c => int.Parse(c.ToString())).ToList();
            var sum = 0;
            for (var i = 0; i < values.Count/2; i++)
            {
                if (values[i] == values[values.Count / 2 + i]) sum += 2 * values[i];
            }
            var expectedResult = 1130;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
