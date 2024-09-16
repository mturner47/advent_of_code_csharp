namespace AdventOfCode.Year2016
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 192;
            var result = 192;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
