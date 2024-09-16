namespace AdventOfCode.Year2015
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 258;
            var result = lines.Count(IsNice);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 53;
            var result = lines.Count(IsSuperNice);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static readonly string _vowels = "aeiou";
        private static readonly List<string> _badStrings = ["ab", "cd", "pq", "xy"];

        private static bool IsNice(string s)
        {
            var vowelCount = 0;
            var hasSeenTwoLettersInARow = false;
            foreach (var badString in _badStrings)
            {
                if (s.Contains(badString)) return false;
            }

            for (var i = 0; i < s.Length; i++)
            {
                if (!hasSeenTwoLettersInARow && i + 1 < s.Length)
                {
                    if (s[i] == s[i + 1]) hasSeenTwoLettersInARow = true;
                }

                if (_vowels.Contains(s[i])) vowelCount++;
            }
            return vowelCount >= 3 && hasSeenTwoLettersInARow;
        }

        private static bool IsSuperNice(string s)
        {
            var hasFoundLetterPair = false;
            var hasFoundSandwichPattern = false;
            for (var i = 0; i < s.Length; i++)
            {
                if (!hasFoundSandwichPattern && i + 2 < s.Length)
                {
                    if (s[i] == s[i + 2]) hasFoundSandwichPattern = true;
                }

                if (!hasFoundLetterPair && i + 3 < s.Length)
                {
                    var letterPair = ("" + s[i]) + s[i + 1];
                    hasFoundLetterPair = s.IndexOf(letterPair, i + 2) > -1;
                }
            }
            return hasFoundLetterPair && hasFoundSandwichPattern;

        }
    }
}
