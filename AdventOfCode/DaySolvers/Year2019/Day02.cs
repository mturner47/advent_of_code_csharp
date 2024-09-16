using AdventOfCode.DaySolvers.Year2019;

namespace AdventOfCode.Year2019
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var program = lines[0].Split(",").Select(long.Parse).ToList();

            program[1] = 12;
            program[2] = 2;

            var expectedResult = 3895705;
            var result = IntCodeComputer.ParseAndRunProgram(program).FinalProgramState[0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var originalProgram = lines[0].Split(",").Select(long.Parse).ToList();
            var desiredResult = 19690720;
            var expectedResult = 6417;
            var result = GetPositionOfDesiredState(originalProgram, desiredResult);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static long GetPositionOfDesiredState(List<long> originalProgram, long desiredResult)
        {
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var program = originalProgram.ToList();
                    program[1] = i;
                    program[2] = j;

                    var result = IntCodeComputer.ParseAndRunProgram(program).FinalProgramState[0];
                    if (result == desiredResult)
                    {
                        return (100 * i) + j;
                    }
                }
            }
            throw new Exception("Something broke");
        }
    }
}
