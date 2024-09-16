namespace AdventOfCode.Year2022
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var monkeys = ConvertInputToMonkeys(lines);
            for (var i = 0; i < 20; i++)
            {
                RunMonkeyRound(monkeys, true);
            }
            var topTwoMonkeyInspectors = monkeys.Values.OrderByDescending(m => m.NumInspections).Take(2).ToList();
            return topTwoMonkeyInspectors[0].NumInspections * topTwoMonkeyInspectors[1].NumInspections;
        }

        public object HardSolution(IList<string> lines)
        {
            var monkeys = ConvertInputToMonkeys(lines);
            for (var i = 0; i < 10000; i++)
            {
                RunMonkeyRound(monkeys, false);
                if (i == 0 || i == 19 || (i + 1)%1000 == 0)
                {
                    Console.WriteLine($"== After round {i + 1} ==");
                    foreach (var key in monkeys.Keys.OrderBy(k => k))
                    {
                        var monkey = monkeys[key];
                        Console.WriteLine($"Monkey {key} inspected items {monkey.NumInspections} times.");
                    }
                    Console.WriteLine();
                }
            }
            var topTwoMonkeyInspectors = monkeys.Values.OrderByDescending(m => m.NumInspections).Take(2).ToList();
            return topTwoMonkeyInspectors[0].NumInspections * topTwoMonkeyInspectors[1].NumInspections;
        }

        private static Dictionary<int, Monkey> ConvertInputToMonkeys(IList<string> lines)
        {
            var monkeys = new Dictionary<int, Monkey>();
            while (lines.Count >= 6)
            {
                var singleMonkeyLines = lines.Take(6).ToList();
                lines = lines.Skip(7).ToList();
                var (monkeyIndex, monkey) = ConvertInputToSingleMonkey(singleMonkeyLines);
                monkeys[monkeyIndex] = monkey;
            }
            return monkeys;
        }

        private static (int, Monkey) ConvertInputToSingleMonkey(IList<string> lines)
        {
            var monkeyIndex = int.Parse(lines[0].Replace("Monkey ", "").Replace(":", ""));
            var items = lines[1].Replace("  Starting items: ", "").Split(", ").Select(i => long.Parse(i)).ToList();
            var (operation, inspectionFormatString) = ConvertOperationLine(lines[2]);
            var testDenominator = int.Parse(lines[3].Replace("  Test: divisible by ", ""));
            var newMonkeyIfTrue = int.Parse(lines[4].Replace("    If true: throw to monkey ", ""));
            var newMonkeyIfFalse = int.Parse(lines[5].Replace("    If false: throw to monkey ", ""));

            return (monkeyIndex, new Monkey
            {
                Items = items,
                Operation = operation,
                TestDenominator = testDenominator,
                NewMonkeyIfTrue = newMonkeyIfTrue,
                NewMonkeyIfFalse = newMonkeyIfFalse,
                InspectionFormatString = inspectionFormatString,
            });
        }

        private static (Func<long, long>, string) ConvertOperationLine(string line)
        {
            var operationLine = line.Replace("  Operation: new = ", "");
            var op = operationLine.Contains(" + ") ? "+" : "*";
            var operationParts = operationLine.Split(" " + op + " ");
            var secondInput = operationParts[1];
            var secondInputIsOld = secondInput == "old";
            var operationName = op == "+"
                ? "increases by"
                : "is multiplied by";
            var secondInputString = secondInputIsOld ? "itself" : secondInput;
            var operationFunction = (long oldValue) =>
            {
                var secondInputValue = secondInputIsOld
                    ? oldValue
                    : long.Parse(secondInput);

                if (op == "+")
                {
                    return oldValue + secondInputValue;
                }
                return oldValue * secondInputValue;
            };

            var operationFormatString = "    Worry level " + operationName + " " + secondInputString + " to {0}";
            return (operationFunction, operationFormatString);
        }

        private static void RunMonkeyRound(Dictionary<int, Monkey> monkeys, bool shouldDecreaseWorry)
        {
            bool shouldPrintDebugCode = false;
            var totalMonkeyTests = monkeys.Values.Select(m => m.TestDenominator).Aggregate(1, (x, y) => x * y);
            foreach (var key in monkeys.Keys.OrderBy(x => x))
            {
                if (shouldPrintDebugCode) Console.WriteLine($"Monkey {key}:");

                var monkey = monkeys[key];
                foreach (var item in monkey.Items)
                {
                    if (shouldPrintDebugCode) Console.WriteLine($"  Monkey inspects an item with a worry level of {item}.");

                    var newItemValue = monkey.Operation(item);
                    if (shouldPrintDebugCode) Console.WriteLine(string.Format(monkey.InspectionFormatString, newItemValue));

                    if (shouldDecreaseWorry)
                    {
                        newItemValue = newItemValue / 3;
                        if (shouldPrintDebugCode) Console.WriteLine($"    Monkey gets bored with item. Worry level is divided by 3 to {newItemValue}.");
                    }
                    newItemValue = newItemValue % totalMonkeyTests;
                    var testPasses = newItemValue % monkey.TestDenominator == 0;
                    var testPassesText = testPasses ? "is" : "is not";
                    if (shouldPrintDebugCode) Console.WriteLine($"    Current worry level {testPassesText} divisible by 23.");

                    var newMonkey = testPasses
                        ? monkey.NewMonkeyIfTrue
                        : monkey.NewMonkeyIfFalse;
                    if (shouldPrintDebugCode) Console.WriteLine($"    Item with worry level {newItemValue} is thrown to monkey {newMonkey}.");

                    monkeys[newMonkey].Items.Add(newItemValue);
                    monkey.NumInspections++;
                }
                monkey.Items.Clear();
            }
        }

        private class Monkey
        {
            public List<long> Items { get; set; } = new List<long>();
            public long NumInspections { get; set; }
            public Func<long, long> Operation { get; set; } = (d) => d;
            public int TestDenominator { get; set; }
            public int NewMonkeyIfTrue { get; set; }
            public int NewMonkeyIfFalse { get; set; }
            public string InspectionFormatString { get; set; } = "";
        }
    }
}
