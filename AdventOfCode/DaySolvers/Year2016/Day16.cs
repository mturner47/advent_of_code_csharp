using System.Text;

namespace AdventOfCode.Year2016
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = lines[0];
            var targetLength = 272;
            var expectedResult = "10111110010110110";
            var result = Solve(input, targetLength);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var input = lines[0];
            var targetLength = 35651584;
            var expectedResult = "01101100001100100";
            var result = Solve(input, targetLength);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string Solve(string input, int targetLength)
        {
            while (input.Length < targetLength)
            {
                input = InputProcessStep(input);
            }
            input = input[..targetLength];

            while (input.Length%2 == 0)
            {
                input = GenerateChecksum(input);
            }
            return input;
        }

        private static string InputProcessStep(string a)
        {
            var b = new string(a.Reverse().ToArray());
            b = b.Replace("1", "2").Replace("0", "1").Replace("2", "0");
            return a + "0" + b;
        }

        private static string GenerateChecksum(string input)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < input.Length - 1; i += 2)
            {
                sb.Append(input[i] == input[i + 1] ? '1' : '0');
            }
            return sb.ToString();
        }
    }
}
