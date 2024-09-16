using AdventOfCode.DaySolvers.Year2019;
using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var program = IntCodeComputer.ParseProgram(lines);

            var permutations = MathHelpers.GetPermutations([0, 1, 2, 3, 4]);
            var maxResult = 0L;
            foreach (var permutation in permutations)
            {
                var a = permutation[0];
                var b = permutation[1];
                var c = permutation[2];
                var d = permutation[3];
                var e = permutation[4];
                var result = RunEasyProgram(program, a, b, c, d, e, false);
                if (maxResult < result)
                {
                    maxResult = result;
                }
            }

            var expectedResult = 338603;
            var pass = expectedResult == maxResult ? "Pass" : "Fail";
            return $"{pass} - {maxResult}";
        }

        public object HardSolution(IList<string> lines)
        {
            var debugMode = DebugMode.Off;
            var program = IntCodeComputer.ParseProgram(lines, debugMode: debugMode);

            var permutations = MathHelpers.GetPermutations([5,6,7,8,9]);
            var maxResult = 0L;
            var maxSignal = "";

            foreach (var permutation in permutations)
            {
                var a = permutation[0];
                var b = permutation[1];
                var c = permutation[2];
                var d = permutation[3];
                var e = permutation[4];
                var result = RunHardProgram(program, a, b, c, d, e, debugMode);
                if (maxResult < result)
                {
                    maxResult = result;
                    maxSignal = $"{a}{b}{c}{d}{e}";
                }
            }

            var expectedResult = 63103596;
            var pass = expectedResult == maxResult ? "Pass" : "Fail";
            return $"{pass} - {maxResult}";
        }

        private long RunEasyProgram(IntCodeInput input, int a, int b, int c, int d, int e, bool debug)
        {
            input.InputList = [a, 0];
            var aOutput = IntCodeComputer.RunProgram(input).Outputs[0];
            input.InputList = [b, aOutput];
            var bOutput = IntCodeComputer.RunProgram(input).Outputs[0];
            input.InputList = [c, bOutput];
            var cOutput = IntCodeComputer.RunProgram(input).Outputs[0];
            input.InputList = [d, cOutput];
            var dOutput = IntCodeComputer.RunProgram(input).Outputs[0];
            input.InputList = [e, dOutput];
            var eOutput = IntCodeComputer.RunProgram(input).Outputs[0];
            return eOutput;
        }

        private static long RunHardProgram(IntCodeInput input, int a, int b, int c, int d, int e, DebugMode debugMode)
        {
            var states = new List<(IntCodeOutput a, IntCodeOutput b, IntCodeOutput c, IntCodeOutput d, IntCodeOutput e)>();
            input.InputList = [a, 0];
            if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram A:");
            var aOutput = IntCodeComputer.RunProgram(input);
            input.InputList = [b, aOutput.Outputs[0]];
            if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram B:");
            var bOutput = IntCodeComputer.RunProgram(input);
            input.InputList = [c, bOutput.Outputs[0]];
            if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram C:");
            var cOutput = IntCodeComputer.RunProgram(input);
            input.InputList = [d, cOutput.Outputs[0]];
            if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram D:");
            var dOutput = IntCodeComputer.RunProgram(input);
            input.InputList = [e, dOutput.Outputs[0]];
            if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram E:");
            var eOutput = IntCodeComputer.RunProgram(input);

            if (debugMode != DebugMode.Off)
            {
                Console.WriteLine($"\n{aOutput.Outputs[0]}, {bOutput.Outputs[0]}, {cOutput.Outputs[0]}, {dOutput.Outputs[0]}, {eOutput.Outputs[0]}");
            }

            var initialState = (aOutput, bOutput, cOutput, dOutput, eOutput);
            states.Add(initialState);

            while (true)
            {
                if (debugMode != DebugMode.Off) Console.WriteLine("\n");
                var priorState = states.Last();
                if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram A:");
                aOutput = priorState.a.PausedAtIndex.HasValue
                    ? IntCodeComputer.RunProgram(priorState.a.FinalProgramState, [priorState.e.Outputs[0]], priorState.a.PausedAtIndex.Value, debugMode:debugMode)
                    : priorState.a;
                if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram B:");
                bOutput = priorState.b.PausedAtIndex.HasValue
                    ? IntCodeComputer.RunProgram(priorState.b.FinalProgramState, [aOutput.Outputs[0]], priorState.b.PausedAtIndex.Value, debugMode: debugMode)
                    : priorState.b;
                if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram C:");
                cOutput = priorState.c.PausedAtIndex.HasValue
                    ? IntCodeComputer.RunProgram(priorState.c.FinalProgramState, [bOutput.Outputs[0]], priorState.c.PausedAtIndex.Value, debugMode: debugMode)
                    : priorState.c;
                if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram D:");
                dOutput = priorState.d.PausedAtIndex.HasValue
                    ? IntCodeComputer.RunProgram(priorState.d.FinalProgramState, [cOutput.Outputs[0]], priorState.d.PausedAtIndex.Value, debugMode: debugMode)
                    : priorState.d;
                if (debugMode != DebugMode.Off) Console.WriteLine("\nProgram E:");
                eOutput = priorState.e.PausedAtIndex.HasValue
                    ? IntCodeComputer.RunProgram(priorState.e.FinalProgramState, [dOutput.Outputs[0]], priorState.e.PausedAtIndex.Value, debugMode: debugMode)
                    : priorState.e;

                if (debugMode != DebugMode.Off)
                {
                    Console.WriteLine($"\n{aOutput.Outputs[0]}, {bOutput.Outputs[0]}, {cOutput.Outputs[0]}, {dOutput.Outputs[0]}, {eOutput.Outputs[0]}");
                }

                if (!eOutput.PausedAtIndex.HasValue) return eOutput.Outputs[0];
                states.Add((aOutput, bOutput, cOutput, dOutput, eOutput));
            }

            throw new NotImplementedException();
        }
    }
}
