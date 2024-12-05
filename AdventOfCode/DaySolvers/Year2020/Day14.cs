namespace AdventOfCode.Year2020
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var values = new Dictionary<string, long>();
            var currentMask = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    currentMask = line.Replace("mask = ", "");
                    continue;
                }

                var parts = line.Replace("mem[", "").Split("] = ");
                var register = parts[0];
                var value = Convert.ToString(long.Parse(parts[1]), 2);
                value = value.PadLeft(currentMask.Length, '0');
                var correctedValue = value.ToArray();
                for (var i = 0; i < currentMask.Length; i++)
                {
                    if (currentMask[i] != 'X') correctedValue[i] = currentMask[i];
                }
                values[register] = Convert.ToInt64(new string(correctedValue), 2);
            }
            var expectedResult = 5055782549997;
            var result = values.Values.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var values = new Dictionary<long, long>();
            var currentMask = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    currentMask = line.Replace("mask = ", "");
                    continue;
                }

                var parts = line.Replace("mem[", "").Split("] = ");
                var value = long.Parse(parts[1]);
                var originalRegister = Convert.ToString(long.Parse(parts[0]), 2).PadLeft(currentMask.Length, '0').ToArray();

                for (var i = 0; i < currentMask.Length; i++)
                {
                    if (currentMask[i] != '0') originalRegister[i] = currentMask[i];
                }
                var registers = GetAllRegisters(originalRegister.ToList()).Select(r => Convert.ToInt64(new string(r.ToArray()), 2));
                foreach (var register in registers)
                {
                    values[register] = value;
                }
            }

            var expectedResult = 4795970362286;
            var result = values.Values.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<List<char>> GetAllRegisters(List<char> register)
        {
            var index = register.IndexOf('X');
            if (index == -1) return [register];
            var zeroReg = register.ToList();
            zeroReg[index] = '0';
            var oneReg = register.ToList();
            oneReg[index] = '1';
            return GetAllRegisters(zeroReg).Concat(GetAllRegisters(oneReg)).ToList();
        }
    }
}
