using AdventOfCode.DaySolvers.Year2019;

namespace AdventOfCode.Year2019
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 14522484;
            var result = IntCodeComputer.ParseAndRunProgram(lines, [1]).Outputs.Last();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 4655956;
            var result = IntCodeComputer.ParseAndRunProgram(lines, [5]).Outputs.First();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
