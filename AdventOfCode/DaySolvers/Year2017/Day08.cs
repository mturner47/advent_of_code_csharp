using System.Text.RegularExpressions;

namespace AdventOfCode.Year2017
{
    internal partial class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var registers = new Dictionary<string, int>();
            var instructions = lines.Select(Parse).ToList();

            foreach (var instruction in instructions)
            {
                var registerToCheckValue = registers.ContainsKey(instruction.RegisterToCheck)
                    ? registers[instruction.RegisterToCheck] :
                    0;

                if (instruction.ConditionFunc(registerToCheckValue, instruction.ComparisonAmount))
                {
                    var registerToModifyValue = registers.ContainsKey(instruction.RegisterToModify)
                        ? registers[instruction.RegisterToModify]
                        : 0;
                    registers[instruction.RegisterToModify] = instruction.ModificationFunc(registerToModifyValue, instruction.AmountToModify);
                }
            }

            var expectedResult = 7296;
            var result = registers.Values.Max();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var registers = new Dictionary<string, int>();
            var instructions = lines.Select(Parse).ToList();
            var maxAmount = 0;

            foreach (var instruction in instructions)
            {
                var registerToCheckValue = registers.ContainsKey(instruction.RegisterToCheck)
                    ? registers[instruction.RegisterToCheck] :
                    0;

                if (instruction.ConditionFunc(registerToCheckValue, instruction.ComparisonAmount))
                {
                    var registerToModifyValue = registers.ContainsKey(instruction.RegisterToModify)
                        ? registers[instruction.RegisterToModify]
                        : 0;
                    var newValue = instruction.ModificationFunc(registerToModifyValue, instruction.AmountToModify);
                    registers[instruction.RegisterToModify] = newValue;
                    if (newValue > maxAmount) maxAmount = newValue;
                }
            }

            var expectedResult = 8186;
            var result = maxAmount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Instruction Parse(string line)
        {
            var regex = InstructionRegex();
            var matches = regex.Matches(line)[0];
            return new Instruction
            {
                RegisterToModify = matches.Groups[1].Value,
                ModificationFunc = matches.Groups[2].Value == "inc"
                    ? (int x, int y) => x + y
                    : (int x, int y) => x - y,
                AmountToModify = int.Parse(matches.Groups[3].Value),
                RegisterToCheck = matches.Groups[4].Value,
                ConditionFunc = matches.Groups[5].Value switch
                {
                    "==" => (int x, int y) => x == y,
                    ">" => (int x, int y) => x > y,
                    ">=" => (int x, int y) => x >= y,
                    "<" => (int x, int y) => x < y,
                    "<=" => (int x, int y) => x <= y,
                    "!=" => (int x, int y) => x != y,
                    _ => throw new NotImplementedException(),
                },
                ComparisonAmount = int.Parse(matches.Groups[6].Value),
            };
        }

        private class Instruction
        {
            public string RegisterToModify = "";
            public required Func<int, int, int> ModificationFunc;
            public int AmountToModify;
            public string RegisterToCheck = "";
            public required Func<int, int, bool> ConditionFunc;
            public int ComparisonAmount;
        }

        [GeneratedRegex(@"^([^ ]+) (inc|dec) (-?\d+) if ([^ ]+) ([^ ]+) (-?\d+)")]
        private static partial Regex InstructionRegex();
    }
}
