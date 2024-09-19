using Helpers.Extensions;

namespace AdventOfCode.Year2020
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var instructions = lines.Select(ConvertLineToInstruction).ToList();

            var visitedIndices = new List<int>();
            var currentIndex = 0;
            var accumulatorValue = 0;

            while (true)
            {
                if (visitedIndices.Contains(currentIndex))
                {
                    break;
                }

                visitedIndices.Add(currentIndex);
                var currentInstruction = instructions[currentIndex];
                (currentIndex, accumulatorValue) = RunInstruction(currentIndex, accumulatorValue, currentInstruction);
            }

            var expectedResult = 1814;
            var result = accumulatorValue;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var instructions = lines.Select(ConvertLineToInstruction).ToList();
            var stateHistory = new List<InstructionState>();
            int? latestAttemptedSwitchIndex = null;
            var currentState = new InstructionState(0, 0);
            Instruction currentInstruction;

            while (true)
            {
                if (currentState.Index >= instructions.Count)
                {
                    break;
                }

                if (stateHistory.Any(vi => vi.Index == currentState.Index))
                {
                    latestAttemptedSwitchIndex ??= stateHistory.Count;

                    while (true)
                    {
                        currentState = stateHistory[latestAttemptedSwitchIndex.Value - 1];
                        stateHistory = stateHistory.GetRange(0, latestAttemptedSwitchIndex.Value - 1);
                        currentInstruction = instructions[currentState.Index];
                        latestAttemptedSwitchIndex--;
                        if (currentInstruction.Operation != Operation.Acc)
                        {
                            var newOperation = currentInstruction.Operation == Operation.Jmp ? Operation.Nop : Operation.Jmp;
                            currentInstruction = currentInstruction with { Operation = newOperation };
                            break;
                        }
                    }
                }
                else
                {
                    stateHistory.Add(currentState);
                    currentInstruction = instructions[currentState.Index];
                }
                currentState = RunInstruction(currentState.Index, currentState.AccumulatorValue, currentInstruction);
            }

            var expectedResult = 1056;
            var result = currentState.AccumulatorValue;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static InstructionState RunInstruction(int currentIndex, int currentAccumulatorValue, Instruction instruction)
        {
            return instruction.Operation switch
            {
                Operation.Acc => new InstructionState(currentIndex + 1, currentAccumulatorValue + instruction.Value),
                Operation.Jmp => new InstructionState(currentIndex + instruction.Value, currentAccumulatorValue),
                Operation.Nop => new InstructionState(currentIndex + 1, currentAccumulatorValue),
                _ => throw new NotImplementedException(),
            };
        }

        private static Instruction ConvertLineToInstruction(string line)
        {
            var lineParts = line.Split(" ");
            var operation = lineParts[0] switch
            {
                "acc" => Operation.Acc,
                "jmp" => Operation.Jmp,
                "nop" => Operation.Nop,
                _ => Operation.Nop,
            };

            var value = lineParts[1].ToNullableInt();
            if (!value.HasValue)
            {
                throw new ArgumentException("Bad data");
            }
            return new Instruction(operation, value.Value);
        }

        private record Instruction(Operation Operation, int Value);

        private record InstructionState(int Index, int AccumulatorValue);

        private enum Operation
        {
            Acc,
            Jmp,
            Nop,
        }
    }
}
