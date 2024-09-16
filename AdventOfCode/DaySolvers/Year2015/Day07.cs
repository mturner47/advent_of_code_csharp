using Helpers.Extensions;

namespace AdventOfCode.Year2015
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 16076;
            var result = GetValue(lines.Select(ParseLine).ToDictionary(i => i.Output, i => i), "a");
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var firstResult = GetValue(lines.Select(ParseLine).ToDictionary(i => i.Output, i => i), "a");
            _knownValues.Clear();
            var newDictionary = lines.Select(ParseLine).ToDictionary(i => i.Output, i => i);
            newDictionary["b"].Input1 = firstResult.ToString();
            var expectedResult = 2797;
            var result = GetValue(newDictionary, "a");
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private readonly static Dictionary<string, uint> _knownValues = [];
        private static uint GetValue(Dictionary<string, Instruction> instructions, string goal)
        {
            var goalInstruction = instructions[goal];
            var input1String = goalInstruction.Input1;
            var input2String = goalInstruction.Input2;
            var input1UInt = input1String.ToNullableUInt() ?? _knownValues.GetNullableValue(input1String);
            var input2UInt = input2String.ToNullableUInt() ?? _knownValues.GetNullableValue(input2String);
            var result = goalInstruction.Type switch
            {
                InstructionType.Assignment => input1UInt ?? GetValue(instructions, input1String),
                InstructionType.LShift => (input1UInt ?? GetValue(instructions, input1String)) << int.Parse(goalInstruction.Input2),
                InstructionType.RShift => (input1UInt ?? GetValue(instructions, input1String)) >> int.Parse(goalInstruction.Input2),
                InstructionType.Not => ~(input1UInt ?? GetValue(instructions, input1String)),
                InstructionType.And => (input1UInt ?? GetValue(instructions, input1String)) & (input2UInt ?? GetValue(instructions, input2String)),
                InstructionType.Or => (input1UInt ?? GetValue(instructions, input1String)) | (input2UInt ?? GetValue(instructions, input2String)),
                _ => throw new NotImplementedException(),
            };
            _knownValues[goal] = result;
            return result;
        }

        private static Instruction ParseLine(string line)
        {
            var parts = line.Split(" -> ");
            var output = parts[1];
            var input = parts[0];
            var inputParts = input.Split(" ");
            string input2 = "";
            string input1;
            InstructionType type;
            if (inputParts.Length == 1)
            {
                type = InstructionType.Assignment;
                input1 = inputParts[0];
            }
            else if (inputParts.Length == 2)
            {
                type = InstructionType.Not;
                input1 = inputParts[1];
            }
            else
            {
                input1 = inputParts[0];
                input2 = inputParts[2];
                type = inputParts[1] switch
                {
                    "AND" => InstructionType.And,
                    "OR" => InstructionType.Or,
                    "LSHIFT" => InstructionType.LShift,
                    "RSHIFT" => InstructionType.RShift,
                    _ => throw new NotImplementedException(),
                };
            }

            return new Instruction
            {
                Type = type,
                Output = output,
                Input1 = input1,
                Input2 = input2,
            };
        }

        private class Instruction
        {
            public InstructionType Type;
            public required string Output;
            public required string Input1;
            public required string Input2;
        }

        private enum InstructionType
        {
            Assignment,
            And,
            Or,
            Not,
            LShift,
            RShift,
        }
    }
}
