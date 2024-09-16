using Helpers.Extensions;

namespace AdventOfCode.Year2022
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var monkeys = lines.Select(Monkey.ParseLine).ToDictionary(m => m.Name, m => m);

            var (result, _) = FindValue("root", monkeys, false);
            return result!.Value;
        }

        public object HardSolution(IList<string> lines)
        {
            var monkeys = lines.Select(Monkey.ParseLine).ToDictionary(m => m.Name, m => m);

            var rootMonkey = monkeys["root"];
            var monkey1Name = rootMonkey.Operation!.Monkey1Name;
            var monkey2Name = rootMonkey.Operation!.Monkey2Name;
            var (monkey1Value, monkey1Func) = FindValue(monkey1Name, monkeys, true);
            var (monkey2Value, monkey2Func) = FindValue(monkey2Name, monkeys, true);
            if (monkey1Value.HasValue && monkey2Func != null) return monkey2Func(monkey1Value.Value);
            if (monkey2Value.HasValue && monkey1Func != null) return monkey1Func(monkey2Value.Value);
            return 0;
        }

        private static (double?, Func<double, double>?) FindValue(string monkeyName, Dictionary<string, Monkey> monkeys, bool humanIsSpecial)
        {
            var monkey = monkeys[monkeyName];
            if (humanIsSpecial && monkeyName == "humn")
            {
                return (null, v => v);
            }

            if (monkey.Value.HasValue) return (monkey.Value.Value, null);

            var operation = monkey.Operation;
            var (monkey1Value, monkey1Func) = FindValue(operation!.Monkey1Name, monkeys, humanIsSpecial);
            var (monkey2Value, monkey2Func) = FindValue(operation!.Monkey2Name, monkeys, humanIsSpecial);
            var op = operation!.Operator;
            if (monkey1Value.HasValue && monkey2Value.HasValue)
            {
                var value = op switch
                {
                    '+' => monkey1Value + monkey2Value,
                    '-' => monkey1Value - monkey2Value,
                    '*' => monkey1Value * monkey2Value,
                    '/' => monkey1Value / monkey2Value,
                    _ => throw new NotImplementedException(),
                };
                monkey.Value = value;
                return (value, null);
            }
            else
            {
                if (monkey1Value.HasValue && monkey2Func != null)
                {
                    Func<double, double> resultFunc = op switch
                    {
                        '+' => (v => monkey2Func(v - monkey1Value.Value)),
                        '-' => (v => monkey2Func(monkey1Value.Value - v)),
                        '*' => (v => monkey2Func(v / monkey1Value.Value)),
                        '/' => (v => monkey2Func(monkey1Value.Value / v)),
                        _ => throw new NotImplementedException(),
                    };
                    return (null, resultFunc);
                }

                if (monkey2Value.HasValue && monkey1Func != null)
                {
                    Func<double, double> resultFunc = op switch
                    {
                        '+' => (v => monkey1Func(v - monkey2Value.Value)),
                        '-' => (v => monkey1Func(monkey2Value.Value + v)),
                        '*' => (v => monkey1Func(v / monkey2Value.Value)),
                        '/' => (v => monkey1Func(v * monkey2Value.Value)),
                        _ => throw new NotImplementedException(),
                    };
                    return (null, resultFunc);
                }
                throw new NotImplementedException();
            }
        }

        private class Monkey
        {
            public string Name { get; set; } = "";
            public double? Value { get; set; }
            public Operation? Operation { get; set; }

            public static Monkey ParseLine(string line)
            {
                var parts = line.Split(": ");
                var name = parts[0];
                var value = parts[1].ToNullableInt();
                if (value.HasValue)
                {
                    return new Monkey { Name = name, Value = value.Value };
                }

                var operation = Operation.ParseOperation(parts[1]);
                return new Monkey { Name = name, Operation = operation };
            }
        }

        private class Operation
        {
            public string Monkey1Name { get; init; } = "";
            public string Monkey2Name { get; init; } = "";
            public char Operator { get; init; }

            public static Operation ParseOperation(string operationLine)
            {
                var parts = operationLine.Split(" ");
                return new Operation
                {
                    Monkey1Name = parts[0],
                    Monkey2Name = parts[2],
                    Operator = parts[1][0],
                };
            }
        }
    }
}
