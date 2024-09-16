namespace AdventOfCode.Year2016
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            var letterCounts = new List<Dictionary<char, int>>();
            for (var i = 0; i < lines[0].Length; i++)
            {
                letterCounts.Add(alphabet.ToDictionary(c => c, _ => 0));
            }

            foreach (var line in lines)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    letterCounts[i][c]++;
                }
            }

            var expectedResult = "qoclwvah";
            var result = string.Join("", letterCounts.Select(l => l.OrderByDescending(kvp => kvp.Value).First().Key));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            var letterCounts = new List<Dictionary<char, int>>();
            for (var i = 0; i < lines[0].Length; i++)
            {
                letterCounts.Add(alphabet.ToDictionary(c => c, _ => 0));
            }

            foreach (var line in lines)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    letterCounts[i][c]++;
                }
            }

            var expectedResult = "ryrgviuv";
            var result = string.Join("", letterCounts.Select(l => l.OrderBy(kvp => kvp.Value).First().Key));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
