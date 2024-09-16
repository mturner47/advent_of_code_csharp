using AdventOfCode.DaySolvers.Year2019;
using System.Net.Http.Headers;

namespace AdventOfCode.Year2019
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var output = IntCodeComputer.ParseAndRunProgram(lines);

            while (!output.HaltedExecution)
            {
                var inputs = new List<string>
                {
                    "NOT C J",
                    "AND D J",
                    "NOT A T",
                    "OR T J",
                    "WALK",
                };
                output = IntCodeComputer.RunProgram(output.FinalProgramState, ConvertStringToInstructions(inputs), output.PausedAtIndex ?? 0, output.RelativeBase);
            }

            var expectedResult = 19358416;
            var result = output.Outputs.Last();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var output = IntCodeComputer.ParseAndRunProgram(lines);

            while (!output.HaltedExecution)
            {
                var inputs = new List<string>
                {
                    "NOT C J",
                    "AND D J",
                    "AND H J",
                    "NOT B T",
                    "AND D T",
                    "OR T J",
                    "NOT A T",
                    "OR T J",
                    "RUN",
                };
                output = IntCodeComputer.RunProgram(output.FinalProgramState, ConvertStringToInstructions(inputs), output.PausedAtIndex ?? 0, output.RelativeBase);
            }

            var expectedResult = 1144641747;
            var result = output.Outputs.Last();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<long> ConvertStringToInstructions(List<string> inputStrings)
        {
            return (string.Join('\n', inputStrings) + '\n').Select(c => (long)c).ToList();
        }
    }
}
