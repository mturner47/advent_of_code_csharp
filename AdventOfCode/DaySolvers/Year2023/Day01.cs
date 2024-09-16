namespace AdventOfCode.Year2023
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.Sum(GetNumberFromLine);
        }

        public object HardSolution(IList<string> lines)
        {
            return lines.Select(FixedString).Sum(GetNumberFromLine);
        }

        private Dictionary<string, string> _numbers = new Dictionary<string, string>
        {
            { "one", "o1e" },
            { "two", "t2o" },
            { "three", "t3e" },
            { "four", "f4r" },
            { "five", "f5e" },
            { "six", "s6x" },
            { "seven", "s7n" },
            { "eight", "e8t" },
            { "nine", "n9e" },
        };

        private string FixedString(string s)
        {
            for (var i = 0; i < s.Length; i++)
            {
                foreach (var kvp in _numbers)
                {
                    if (s.IndexOf(kvp.Key) == i)
                    {
                        return FixedString(s.Replace(kvp.Key, kvp.Value));
                    }
                }
            }
            return s;
        }

        private int GetNumberFromLine(string line)
        {
            var justNumbers = new string(line.Where(char.IsNumber).ToArray());
            return int.Parse(justNumbers[..1] + justNumbers[^1..]);
        }
    }
}
