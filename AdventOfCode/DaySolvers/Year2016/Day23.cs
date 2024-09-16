using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day23 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            Dictionary<string, int> registers = new() { { "a", 7 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, };
            RunProgram(registers, lines);
            var expectedResult = 10365;
            var result = registers["a"];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            Dictionary<string, int> registers = new() { { "a", 12 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 } };
            RunProgram(registers, lines);

            var expectedResult = 479006925; // 71 * 75 + MathHelpers.Factorial(12)
            var result = registers["a"];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Dictionary<string, int> RunProgram(Dictionary<string, int> registers, IList<string> lines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                //Console.WriteLine($"{i} - {line}");

                if (line.StartsWith("tgl"))
                {
                    var value = line.Replace("tgl ", "");
                    var indexToChange = i + (registers.ContainsKey(value) ? registers[value] : int.Parse(value));
                    if (indexToChange < lines.Count)
                    {
                        var lineToChange = lines[indexToChange];
                        if (lineToChange.Contains("jnz")) lines[indexToChange] = lineToChange.Replace("jnz", "cpy");
                        if (lineToChange.Contains("cpy")) lines[indexToChange] = lineToChange.Replace("cpy", "jnz");
                        if (lineToChange.Contains("inc")) lines[indexToChange] = lineToChange.Replace("inc", "dec");
                        if (lineToChange.Contains("dec")) lines[indexToChange] = lineToChange.Replace("dec", "inc");
                        if (lineToChange.Contains("tgl")) lines[indexToChange] = lineToChange.Replace("tgl", "inc");
                    }
                }
                else if (line.StartsWith("add"))
                {
                    var parts = line.Replace("add ", "").Split(" ");
                    if (registers.ContainsKey(parts[1]))
                    {
                        registers[parts[1]] += registers.ContainsKey(parts[0]) ? registers[parts[0]] : int.Parse(parts[0]);
                    }
                }
                else if (line.StartsWith("mul"))
                {
                    var parts = line.Replace("mul ", "").Split(" ");
                    if (registers.ContainsKey(parts[1]))
                    {
                        registers[parts[1]] *= registers.ContainsKey(parts[0]) ? registers[parts[0]] : int.Parse(parts[0]);
                    }
                }
                else if (line.StartsWith("cpy"))
                {
                    var parts = line.Replace("cpy ", "").Split(" ");
                    if (registers.ContainsKey(parts[1]))
                    {
                        registers[parts[1]] = registers.ContainsKey(parts[0]) ? registers[parts[0]] : int.Parse(parts[0]);
                    }
                }
                else if (line.StartsWith("inc"))
                {
                    var registerLetter = line.Replace("inc ", "");
                    if (registers.ContainsKey(registerLetter))
                    {
                        registers[registerLetter]++;
                    }
                }
                else if (line.StartsWith("dec"))
                {
                    var registerLetter = line.Replace("dec ", "");
                    if (registers.ContainsKey(registerLetter))
                    {
                        registers[registerLetter]--;
                    }
                }
                else if (line.StartsWith("jnz"))
                {
                    var parts = line.Replace("jnz ", "").Split(" ");
                    var comparisonValue = registers.ContainsKey(parts[0]) ? registers[parts[0]] : int.Parse(parts[0]);
                    if (comparisonValue != 0)
                    {
                        i += (registers.ContainsKey(parts[1]) ? registers[parts[1]] : int.Parse(parts[1])) - 1;
                    }
                }
                //Console.WriteLine($"Next i: {i + 1}; Registers: a={registers["a"]}; b={registers["b"]}; c={registers["c"]}; d={registers["d"]}");
            }
            return registers;
        }
    }
}
