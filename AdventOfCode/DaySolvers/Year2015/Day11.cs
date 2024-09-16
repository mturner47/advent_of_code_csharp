namespace AdventOfCode.Year2015
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = "cqjxxyzz";
            var result = FindNextPassword(lines[0]);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = "cqkaabcc";
            var result = FindNextPassword(FindNextPassword(lines[0]));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static readonly char[] _badLetters = ['i', 'l', 'o'];
        private static readonly char _maxLetter = 'z';
        private static readonly char _minLetter = 'a';

        private static string FindNextPassword(string password)
        {
            while (true)
            {
                password = IncrementPassword(password);
                if (IsValidPassword(password)) return password;
            }
        }

        private static string IncrementPassword(string password)
        {
            var newPassword = password.ToArray();
            for (var i = newPassword.Length - 1; i >= 0; i--)
            {
                if (newPassword[i] == _maxLetter)
                {
                    newPassword[i] = _minLetter;
                }
                else
                {
                    newPassword[i]++;
                    if (_badLetters.Contains(newPassword[i])) newPassword[i]++;
                    break;
                }
            }
            return new string(newPassword);
        }

        private static bool IsValidPassword(string password)
        {
            var foundIncreasingStraight = false;
            var firstDoubleLetter = 0;
            var foundBothDoubleLetters = false;
            for (var i = 0; i < password.Length; i++)
            {
                var c = password[i];

                if (!foundIncreasingStraight)
                {
                    if (password.Length - i < 3) break;
                    if (password[i + 1] == c + 1 && password[i + 2] == c + 2) foundIncreasingStraight = true;
                }

                if (!foundBothDoubleLetters)
                {
                    if (password.Length - i < 2) break;
                    if (firstDoubleLetter != c && c == password[i + 1])
                    {
                        if (firstDoubleLetter == 0) firstDoubleLetter = c;
                        else foundBothDoubleLetters = true;
                    }
                }
            }
            return foundIncreasingStraight && foundBothDoubleLetters;
        }
    }
}
