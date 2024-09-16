namespace AdventOfCode.Year2017
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 466;
            var result = lines.Count(IsValidEasy);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 251;
            var result = lines.Count(IsValidHard);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static bool IsValidEasy(string line)
        {
            var wordsInPhrase = line.Split(' ').ToList();
            return wordsInPhrase.Distinct().Count() == wordsInPhrase.Count;
        }

        private static bool IsValidHard(string line)
        {
            var wordsInPhrase = line.Split(' ').Select(s => new string(s.OrderBy(c => c).ToArray())).ToList();
            return wordsInPhrase.Distinct().Count() == wordsInPhrase.Count;
        }
    }
}
