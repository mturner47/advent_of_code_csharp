using System.Reflection.Emit;

namespace AdventOfCode.Year2018
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var samples = new List<(List<int> input, int opCode, int a, int b, int c, List<int> output)>();

            var i = 0;
            while (lines[i].StartsWith("Before"))
            {
                samples.Add(Parse(lines.Skip(i).Take(3).ToList()));
                i += 4;
            }

            var operations = new List<(string name, Func<List<int>, int, int, int> func)>
            {
                ("AddR", AddR), ("AddI", AddI), ("MulR", MulR), ("MulI", MulI), ("BanR", BanR), ("BanI", BanI), ("BorR", BorR), ("BorI", BorI), ("SetR", SetR), ("SetI", SetI), ("GtIR", GtIR), ("GtRI", GtRI), ("GtRR", GtRR), ("EqIR", EqIR), ("EqRI", EqRI), ("EqRR", EqRR),
            };

            var opCounts = 0;
            foreach (var (input, opCode, a, b, c, output) in samples)
            {
                var possibleOps = GetPossibleOpCodes(input, opCode, a, b, c, output, operations);
                if (possibleOps.Count >= 3) opCounts++;
            }

            var expectedResult = 624;
            var result = opCounts;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var samples = new List<(List<int> input, int opCode, int a, int b, int c, List<int> output)>();

            var i = 0;
            while (lines[i].StartsWith("Before"))
            {
                samples.Add(Parse(lines.Skip(i).Take(3).ToList()));
                i += 4;
            }

            var operations = new List<(string name, Func<List<int>, int, int, int> func)>
            {
                ("AddR", AddR), ("AddI", AddI), ("MulR", MulR), ("MulI", MulI), ("BanR", BanR), ("BanI", BanI), ("BorR", BorR), ("BorI", BorI), ("SetR", SetR), ("SetI", SetI), ("GtIR", GtIR), ("GtRI", GtRI), ("GtRR", GtRR), ("EqIR", EqIR), ("EqRI", EqRI), ("EqRR", EqRR),
            };

            var opDict = operations.ToDictionary(o => o.name, o => o.func);

            var opCodeFunctions = Enumerable.Range(0, 16).ToDictionary(o => o, _ => operations.Select(o => o.name).ToList());

            foreach (var (input, opCode, a, b, c, output) in samples)
            {
                var possibleOps = GetPossibleOpCodes(input, opCode, a, b, c, output, operations);
                opCodeFunctions[opCode] = opCodeFunctions[opCode].Intersect(possibleOps).ToList();
            }

            while (opCodeFunctions.Values.Any(v => v.Count > 1))
            {
                foreach (var kvp in opCodeFunctions)
                {
                    if (kvp.Value.Count == 1)
                    {
                        foreach (var kvp2 in opCodeFunctions)
                        {
                            if (kvp.Key == kvp2.Key) continue;
                            kvp2.Value.Remove(kvp.Value[0]);
                        }
                    }
                }
            }

            var finalOpCodes = opCodeFunctions.ToDictionary(kvp => kvp.Key, kvp => opDict[kvp.Value[0]]);

            i += 2;
            var registers = new List<int> { 0, 0, 0, 0 };
            while (i < lines.Count)
            {
                var parts = lines[i].Split(" ");
                var (opCode, a, b, c) = (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
                registers[c] = finalOpCodes[opCode](registers, a, b);
                i++;
            }

            var expectedResult = 584;
            var result = registers[0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (List<int> input, int opCode, int a, int b, int c, List<int> output) Parse(List<string> lines)
        {
            var input = lines[0].Replace("Before: [", "").Replace("]", "").Split(", ").Select(int.Parse).ToList();
            var output = lines[2].Replace("After:  [", "").Replace("]", "").Split(", ").Select(int.Parse).ToList();
            var opParts = lines[1].Split(" ");
            return (input, int.Parse(opParts[0]), int.Parse(opParts[1]), int.Parse(opParts[2]), int.Parse(opParts[3]), output);
        }

        private static List<string> GetPossibleOpCodes(List<int> input, int _, int a, int b, int c, List<int> output, List<(string name, Func<List<int>, int, int, int> func)> operations)
        {
            var expectedOutput = output[c];
            var possibleOps = new List<string>();
            foreach (var (name, func) in operations)
            {
                var actualOutput = func(input, a, b);
                if (expectedOutput == actualOutput) possibleOps.Add(name);
            }
            return possibleOps;
        }

        private static int AddR(List<int> r, int a, int b) => r[a] + r[b];
        private static int AddI(List<int> r, int a, int b) => r[a] + b;
        private static int MulR(List<int> r, int a, int b) => r[a] * r[b];
        private static int MulI(List<int> r, int a, int b) => r[a] * b;
        private static int BanR(List<int> r, int a, int b) => r[a] & r[b];
        private static int BanI(List<int> r, int a, int b) => r[a] & b;
        private static int BorR(List<int> r, int a, int b) => r[a] | r[b];
        private static int BorI(List<int> r, int a, int b) => r[a] | b;
        private static int SetR(List<int> r, int a, int b) => r[a];
        private static int SetI(List<int> r, int a, int b) => a;
        private static int GtIR(List<int> r, int a, int b) => a > r[b] ? 1 : 0;
        private static int GtRI(List<int> r, int a, int b) => r[a] > b ? 1 : 0;
        private static int GtRR(List<int> r, int a, int b) => r[a] > r[b] ? 1 : 0;
        private static int EqIR(List<int> r, int a, int b) => a == r[b] ? 1 : 0;
        private static int EqRI(List<int> r, int a, int b) => r[a] == b ? 1 : 0;
        private static int EqRR(List<int> r, int a, int b) => r[a] == r[b] ? 1 : 0;
    }
}
