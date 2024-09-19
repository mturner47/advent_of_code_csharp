namespace AdventOfCode.Year2018
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var startingRegisters = Enumerable.Repeat(0L, 6).ToList();
            var expectedResult = 920;
            var result = Solve(startingRegisters, lines)[0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var startingRegisters = Enumerable.Repeat(0L, 6).ToList();
            startingRegisters[0] = 1;
            var expectedResult = 11151360;
            var result = 1 + 23 + 79 + 5807 + (23 * 79) + 23 * 5807 + 79 * 5807 + 23 * 79 * 5807;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private List<long> Solve(List<long> registers, IList<string> lines)
        {
            var ip = int.Parse(lines[0].Replace("#ip ", ""));
            var instructions = lines.Skip(1).Select(Parse).ToList();
            var iterationCount = 0;
            while (registers[ip] >= 0 && registers[ip] < instructions.Count)
            {
                var (name, func, a, b, c) = instructions[(int)registers[ip]];
                registers[(int)c] = func(registers, a, b);
                registers[ip]++;
                iterationCount++;
            }
            return registers;
        }

        private (string name, Func<List<long>, long, long, long> func, long a, long b, long c) Parse(string line)
        {
            var parts = line.Split(' ');
            Func<List<long>, long, long, long> func = parts[0] switch
            {
                "addr" => AddR,
                "addi" => AddI,
                "mulr" => MulR,
                "muli" => MulI,
                "banr" => BanR,
                "bani" => BanI,
                "borr" => BorR,
                "bori" => BorI,
                "setr" => SetR,
                "seti" => SetI,
                "gtir" => GtIR,
                "gtri" => GtRI,
                "gtrr" => GtRR,
                "eqir" => EqIR,
                "eqri" => EqRI,
                "eqrr" => EqRR,
                _ => throw new NotImplementedException(),
            };
            return (parts[0], func, long.Parse(parts[1]), long.Parse(parts[2]), long.Parse(parts[3]));
        }

        private static long AddR(List<long> r, long a, long b) => r[(int)a] + r[(int)b];
        private static long AddI(List<long> r, long a, long b) => r[(int)a] + b;
        private static long MulR(List<long> r, long a, long b) => r[(int)a] * r[(int)b];
        private static long MulI(List<long> r, long a, long b) => r[(int)a] * b;
        private static long BanR(List<long> r, long a, long b) => r[(int)a] & r[(int)b];
        private static long BanI(List<long> r, long a, long b) => r[(int)a] & b;
        private static long BorR(List<long> r, long a, long b) => r[(int)a] | r[(int)b];
        private static long BorI(List<long> r, long a, long b) => r[(int)a] | b;
        private static long SetR(List<long> r, long a, long b) => r[(int)a];
        private static long SetI(List<long> r, long a, long b) => a;
        private static long GtIR(List<long> r, long a, long b) => a > r[(int)b] ? 1 : 0;
        private static long GtRI(List<long> r, long a, long b) => r[(int)a] > b ? 1 : 0;
        private static long GtRR(List<long> r, long a, long b) => r[(int)a] > r[(int)b] ? 1 : 0;
        private static long EqIR(List<long> r, long a, long b) => a == r[(int)b] ? 1 : 0;
        private static long EqRI(List<long> r, long a, long b) => r[(int)a] == b ? 1 : 0;
        private static long EqRR(List<long> r, long a, long b) => r[(int)a] == r[(int)b] ? 1 : 0;
    }
}
