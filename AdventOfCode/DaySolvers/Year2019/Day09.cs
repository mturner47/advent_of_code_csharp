using AdventOfCode.DaySolvers.Year2019;

namespace AdventOfCode.Year2019
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 3460311188;
            var result = IntCodeComputer.ParseAndRunProgram(lines, inputList:[1]).Outputs[0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 42202;
            var result = IntCodeComputer.ParseAndRunProgram(lines, inputList: [2]).Outputs[0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
