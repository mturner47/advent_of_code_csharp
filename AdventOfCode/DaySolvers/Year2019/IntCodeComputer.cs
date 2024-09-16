namespace AdventOfCode.DaySolvers.Year2019
{
    internal class IntCodeInput
    {
        public Dictionary<long, long> Program { get; set; } = [];
        public IEnumerable<long>? InputList { get; set; } = null;
        public long StartingIndex { get; set; } = 0;
        public DebugMode DebugMode { get; set; } = DebugMode.Off;
        public long RelativeBase { get; set; } = 0;
    }

    internal class IntCodeOutput
    {
        public Dictionary<long, long> FinalProgramState { get; set; } = [];
        public List<long> Outputs { get; set; } = [];
        public long? PausedAtIndex { get; set; } = null;
        public long? RelativeBase { get; set; } = 0;
        public bool HaltedExecution { get; set; } = false;
    }

    public enum DebugMode
    {
        Off,
        Instructions,
        Everything,
    }

    internal class IntCodeComputer
    {
        public static IntCodeOutput ParseAndRunProgram(IList<string> lines, IEnumerable<long>? inputList = null, long startingIndex = 0, long? relativeBase = 0, DebugMode debugMode = DebugMode.Off)
        {
            return RunProgram(ParseProgram(lines, inputList, startingIndex, relativeBase, debugMode));
        }

        public static IntCodeOutput ParseAndRunProgram(List<long> program, IEnumerable<long>? inputList = null, long startingIndex = 0, long? relativeBase = 0, DebugMode debugMode = DebugMode.Off)
        {
            return RunProgram(MakeInput(program, inputList, startingIndex, relativeBase, debugMode));
        }

        public static IntCodeInput ParseProgram(IList<string> lines, IEnumerable<long>? inputList = null, long startingIndex = 0, long? relativeBase = 0, DebugMode debugMode = DebugMode.Off)
        {
            var program = lines[0].Split(",").Select(long.Parse).ToList();
            return MakeInput(program, inputList, startingIndex, relativeBase, debugMode);
        }

        private static IntCodeInput MakeInput(List<long> program, IEnumerable<long>? inputList = null, long startingIndex = 0, long? relativeBase = 0, DebugMode debugMode = DebugMode.Off)
        {
            var programDict = program.Select((value, index) => (value, index)).ToDictionary(x => (long)x.index, x => x.value);
            return MakeInput(programDict, inputList, startingIndex, relativeBase, debugMode);
        }

        private static IntCodeInput MakeInput(Dictionary<long, long> program, IEnumerable<long>? inputList = null, long startingIndex = 0, long? relativeBase = 0, DebugMode debugMode = DebugMode.Off)
        {
            return new IntCodeInput
            {
                Program = program,
                InputList = inputList,
                StartingIndex = startingIndex,
                DebugMode = debugMode,
                RelativeBase = relativeBase ?? 0,
            };
        }

        public static IntCodeOutput RunProgram(Dictionary<long, long> program, IEnumerable<long>? inputList = null, long startingIndex = 0, long? relativeBase = 0, DebugMode debugMode = DebugMode.Off)
        {
            return RunProgram(MakeInput(program, inputList, startingIndex, relativeBase, debugMode));
        }

        public static IntCodeOutput RunProgram(IntCodeInput input)
        {
            var program = input.Program.ToList().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var i = input.StartingIndex;
            var inputQueue = input.InputList != null ? new Queue<long>(input.InputList) : null;
            var outputs = new List<long>();
            long? pausedAtIndex = null;
            var relativeBase = input.RelativeBase;
            var haltedExecution = false;
            while(true)
            {
                if (input.DebugMode == DebugMode.Everything)
                {
                    Console.WriteLine($"{string.Join(",", program)}; i = {i}");
                }

                var operationCode = program[i];
                if (operationCode == (int)OpCode.Halt)
                {
                    if (input.DebugMode != DebugMode.Off)
                    {
                        Console.WriteLine("Halted");
                    }
                    haltedExecution = true;
                    break;
                }
                var numInstructions = 0;
                var parameters = operationCode / 100;
                operationCode %= 100;

                if (!Enum.IsDefined(typeof(OpCode), (int)operationCode))
                {
                    throw new NotImplementedException();
                }

                if (operationCode == (int)OpCode.Input)
                {
                    numInstructions = 1;
                    var instructionIndices = GetInstructionIndices(parameters, i, numInstructions, program, relativeBase);
                    var resultIndex = instructionIndices[0];

                    var result = Input(inputQueue);
                    if (!result.HasValue)
                    {
                        if (input.DebugMode != DebugMode.Off) Console.WriteLine($"Paused at index {i} waiting for input");
                        pausedAtIndex = i;
                        break;
                    }
                    program[resultIndex] = result.Value;

                    if (input.DebugMode != DebugMode.Off) Console.WriteLine($"Input value {program[resultIndex]} into index {resultIndex}");
                    i += numInstructions + 1;
                }

                if (operationCode == (int)OpCode.Output)
                {
                    numInstructions = 1;
                    var instructionIndices = GetInstructionIndices(parameters, i, numInstructions, program, relativeBase);
                    var resultIndex = instructionIndices[0];

                    var result = GetValueAtIndex(program, resultIndex);

                    Output(result, outputs);

                    if (input.DebugMode != DebugMode.Off) Console.WriteLine($"Output {result} (from {resultIndex})");
                    i += numInstructions + 1;
                }

                if (operationCode == (int)OpCode.AdjustRelativeBase)
                {
                    numInstructions = 1;
                    var instructionIndices = GetInstructionIndices(parameters, i, numInstructions, program, relativeBase);
                    var oldRelativeBase = relativeBase;
                    var resultIndex = instructionIndices[0];
                    var relativeBaseChange = GetValueAtIndex(program, resultIndex);
                    relativeBase += relativeBaseChange;

                    if (input.DebugMode != DebugMode.Off) Console.WriteLine($"Updated Relative Base {oldRelativeBase} with {relativeBaseChange} (from {resultIndex}) to {relativeBase}");
                    i += numInstructions + 1;
                }

                if (operationCode == (int)OpCode.Add
                    || operationCode == (int)OpCode.Multiply
                    || operationCode == (int)OpCode.LessThan
                    || operationCode == (int)OpCode.Equals)
                {
                    Func<long, long ,long> opFunc = operationCode switch
                    {
                        (int)OpCode.Add => Add,
                        (int)OpCode.Multiply => Multiply,
                        (int)OpCode.LessThan => LessThan,
                        (int)OpCode.Equals => Equals,
                        _ => throw new NotImplementedException(),
                    };

                    numInstructions = 3;
                    var instructionIndices = GetInstructionIndices(parameters, i, numInstructions, program, relativeBase);
                    var instruction1Index = instructionIndices[0];
                    var instruction1Value = GetValueAtIndex(program, instruction1Index);

                    var instruction2Index = instructionIndices[1];
                    var instruction2Value = GetValueAtIndex(program, instruction2Index);

                    var resultIndex = instructionIndices[2];

                    var opResult = opFunc(instruction1Value, instruction2Value);
                    program[resultIndex] = opResult;

                    if (input.DebugMode != DebugMode.Off)
                    {
                        var opDescription = operationCode switch
                        {
                            (int)OpCode.Add => "+",
                            (int)OpCode.Multiply => "*",
                            (int)OpCode.LessThan => "<",
                            (int)OpCode.Equals => "==",
                            _ => throw new NotImplementedException(),
                        };

                        Console.WriteLine($"{instruction1Value} (from {instruction1Index}) {opDescription} {instruction2Value} (from {instruction2Index}) = {opResult}, stored at {resultIndex}");
                    }

                    i += numInstructions + 1;
                }

                if (operationCode == (int)OpCode.JumpIfTrue || operationCode == (int)OpCode.JumpIfFalse)
                {
                    Func<long, bool> opFunc = (operationCode == (int)OpCode.JumpIfTrue)
                        ? JumpIfTrue
                        : JumpIfFalse;

                    numInstructions = 2;
                    var instructionIndices = GetInstructionIndices(parameters, i, numInstructions, program, relativeBase);
                    var instruction1Index = instructionIndices[0];
                    var instruction1Value = GetValueAtIndex(program, instruction1Index);

                    var resultIndex = GetValueAtIndex(program, instructionIndices[1]);

                    var shouldJump = opFunc(instruction1Value);

                    if (input.DebugMode != DebugMode.Off)
                    {
                        var jumpDescription = operationCode == (int)OpCode.JumpIfTrue ? "JumpIfTrue" : "JumpIfFalse";
                        if (shouldJump)
                        {
                            Console.WriteLine($"{jumpDescription} {instruction1Value} (from {instruction1Index}) evaluated true, jumped to {resultIndex}");
                        }
                        else
                        {
                            Console.WriteLine($"{jumpDescription} {instruction1Value} (from {instruction1Index}) evaluated false, did not jump");
                        }
                    }

                    if (shouldJump)
                    {
                        i = (int)resultIndex;
                    }
                    else
                    {
                        i += numInstructions + 1;
                    }
                }
            }

            return new IntCodeOutput
            {
                FinalProgramState = program,
                Outputs = outputs,
                PausedAtIndex = pausedAtIndex,
                RelativeBase = relativeBase,
                HaltedExecution = haltedExecution,
            };
        }

        private static List<long> GetInstructionIndices(long parameters, long i, int numInstructions, Dictionary<long, long> program, long relativeBase)
        {
            var indices = new List<long>();
            for (var x = 0; x < numInstructions; x++)
            {
                var mode = (InstructionMode)(parameters % 10);
                var index = mode switch
                {
                    InstructionMode.Position => GetValueAtIndex(program, i + x + 1),
                    InstructionMode.Immediate => i + x + 1,
                    InstructionMode.Relative => GetValueAtIndex(program, i + x + 1) + relativeBase,
                    _ => throw new NotImplementedException(),
                };
                indices.Add(index);
                parameters /= 10;
            }
            return indices;
        }

        private static long Add(long x, long y) => x + y;

        private static long Multiply(long x, long y) => x * y;

        private static long? Input(Queue<long>? inputs)
        {
            if (inputs != null && inputs.Count > 0)
            {
                return inputs.Dequeue();
            }
            else return null;
        }

        private static void Output(long value, List<long> outputs)
        {
            outputs.Add(value);
        }

        private static bool JumpIfTrue(long value)
        {
            return value != 0;
        }

        private static bool JumpIfFalse(long value)
        {
            return value == 0;
        }

        private static long LessThan(long first, long second)
        {
            return first < second ? 1 : 0;
        }

        private static long Equals(long first, long second)
        {
            return first == second ? 1 : 0;
        }

        private static long GetValueAtIndex(Dictionary<long, long> dictionary, long index)
        {
            return dictionary.TryGetValue(index, out long value) ? value : 0;
        }

        private enum OpCode
        {
            Halt = 99,
            Add = 1,
            Multiply = 2,
            Input = 3,
            Output = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            AdjustRelativeBase = 9,
        }

        private enum InstructionMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }
    }
}
