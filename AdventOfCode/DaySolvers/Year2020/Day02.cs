namespace AdventOfCode.Year2020
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var passwords = lines.Select(Parse).ToList();
            var expectedResult = 398;
            var result = passwords.Count(p => IsValidEasy(p.lower, p.upper, p.letter, p.password));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var passwords = lines.Select(Parse).ToList();
            var expectedResult = 562;
            var result = passwords.Count(p => IsValidHard(p.lower, p.upper, p.letter, p.password));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int lower, int upper, char letter, string password) Parse(string line)
        {
            var parts = line.Split(": ");
            var password = parts[1];
            var parts2 = parts[0].Split(' ');
            var letter = parts2[1][0];
            var parts3 = parts2[0].Split('-');
            var lower = int.Parse(parts3[0]);
            var upper = int.Parse(parts3[1]);
            return (lower, upper, letter, password);
        }

        private static bool IsValidEasy(int lower, int upper, char letter, string password)
        {
            var count = password.Count(c => c == letter);
            return count >= lower && count <= upper;
        }

        private static bool IsValidHard(int indexA, int indexB, char letter, string password)
        {
            var letterA = password[indexA - 1];
            var letterB = password[indexB - 1];
            return (letterA == letter && letterB != letter) || (letterB == letter && letterA != letter);
        }

    }
}
