namespace AdventOfCode.Year2016
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            Dictionary<string, int> registers = new() { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, };
            RunProgram(registers, lines);

            var expectedResult = 318083;
            var result = registers["a"];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            Dictionary<string, int> registers = new() { { "a", 0 }, { "b", 0 }, { "c", 1 }, { "d", 0 }, };
            RunProgram(registers, lines);

            var expectedResult = 9227737;
            var result = registers["a"];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Dictionary<string, int> RunProgram(Dictionary<string, int> registers, IList<string> lines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("cpy"))
                {
                    var parts = line.Replace("cpy ", "").Split(" ");
                    registers[parts[1]] = registers.ContainsKey(parts[0]) ? registers[parts[0]] : int.Parse(parts[0]);
                    continue;
                }

                if (line.StartsWith("inc"))
                {
                    registers[line.Replace("inc ", "")]++;
                    continue;
                }

                if (line.StartsWith("dec"))
                {
                    registers[line.Replace("dec ", "")]--;
                    continue;
                }

                if (line.StartsWith("jnz"))
                {
                    var parts = line.Replace("jnz ", "").Split(" ");
                    var comparisonValue = registers.ContainsKey(parts[0]) ? registers[parts[0]] : int.Parse(parts[0]);
                    if (comparisonValue != 0)
                    {
                        i += int.Parse(parts[1]) - 1;
                    }
                }
            }
            return registers;
        }
    }
}
