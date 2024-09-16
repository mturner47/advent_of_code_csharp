namespace AdventOfCode.Year2016
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var decompressedInput = "";
            var compressedInput = lines[0];
            for (var i = 0; i < compressedInput.Length; i++)
            {
                if (compressedInput[i] == '(')
                {
                    var closingIndex = compressedInput.IndexOf(')', i);
                    var substring = compressedInput.Substring(i + 1, closingIndex - i);
                    var parts = substring.Split("x");
                    var numChars = int.Parse(parts[0]);
                    var sequence = compressedInput.Substring(closingIndex + 1, numChars);
                    var numTimesToRepeat = int.Parse(parts[1].Replace(")", ""));
                    for (var j = 0; j < numTimesToRepeat; j++)
                    {
                        decompressedInput += sequence;
                    }
                    i = closingIndex + numChars;
                }
                else decompressedInput += compressedInput[i];
            }

            var expectedResult = 183269;
            var result = decompressedInput.Length;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 11317278863;
            var result = GetLength(lines[0]);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static long GetLength(string s)
        {
            if (s.Length == 0) return 0;
            if (s[0] != '(') return 1 + GetLength(s[1..]);
            var closingIndex = s.IndexOf(')');
            var substring = s[1..closingIndex];
            var parts = substring.Split("x");
            var numChars = int.Parse(parts[0]);
            var sequence = s[(closingIndex + 1)..(closingIndex + 1 + numChars)];
            var numTimesToRepeat = int.Parse(parts[1].Replace(")", ""));
            return GetLength(sequence)*numTimesToRepeat + GetLength(s[(closingIndex + numChars + 1)..]);
        }
    }
}
