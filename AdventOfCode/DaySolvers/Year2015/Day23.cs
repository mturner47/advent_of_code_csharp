namespace AdventOfCode.Year2015
{
    internal class Day23 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var commands = ParseList(lines);

            var registers = new Dictionary<char, int> { { 'a', 0 }, { 'b', 0 } };

            var expectedResult = 255;
            var result = RunProgram(registers, commands)['b'];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var commands = ParseList(lines);

            var registers = new Dictionary<char, int> { { 'a', 1 }, { 'b', 0 } };

            var expectedResult = -1;
            var result = RunProgram(registers, commands)['b'];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public static Dictionary<char, int> RunProgram(Dictionary<char, int> registers, List<(string command, char register, int offset)> commands)
        {
            var currentIndex = 0;
            while (currentIndex < commands.Count)
            {
                var (command, register, offset) = commands[currentIndex];
                if (command == "hlf")
                {
                    registers[register] /= 2;
                    currentIndex++;
                }
                else if (command == "tpl")
                {
                    registers[register] *= 3;
                    currentIndex++;
                }
                else if (command == "inc")
                {
                    registers[register] += 1;
                    currentIndex++;
                }
                else if (command == "jmp")
                {
                    currentIndex += offset;
                }
                else if (command == "jie")
                {
                    currentIndex += registers[register] % 2 == 0
                        ? offset
                        : 1;
                }
                else if (command == "jio")
                {
                    currentIndex += registers[register] == 1
                        ? offset
                        : 1;
                }
            }
            return registers;
        }

        private List<(string command, char register, int offset)> ParseList(IList<string> lines)
        {
            return lines.Select(l =>
            {
                var lineParts = l.Split(" ");
                var command = lineParts[0];
                char register = 'c';
                int offset = 0;
                if (command == "hlf" || command == "tpl" || command == "inc" || command == "jmp")
                {
                    if (command == "jmp")
                    {
                        offset = int.Parse(lineParts[1]);
                    }
                    else
                    {
                        register = lineParts[1][0];
                    }
                }
                else
                {
                    register = lineParts[1][0];
                    offset = int.Parse(lineParts[2]);
                }
                return (command, register, offset);
            }).ToList();
        }
    }
}
