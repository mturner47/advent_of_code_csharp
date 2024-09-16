using System.Text;

namespace AdventOfCode.Year2015
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = lines[0];
            for (var i = 0; i < 40; i++)
            {
                input = LookAndSay(input);
            }
            var expectedResult = 492982;
            var result = input.Length;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var input = lines[0];
            for (var i = 0; i < 50; i++)
            {
                input = LookAndSay(input);
            }
            var expectedResult = 492982;
            var result = input.Length;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string LookAndSay(string input)
        {
            var sb = new StringBuilder();
            var i = 0;
            while (i < input.Length)
            {
                var c = input[i];
                var count = 1;
                while (i + count < input.Length && input[i + count] == c)
                {
                    count++;
                }
                sb.Append(count);
                sb.Append(c);
                i += count;
            }
            return sb.ToString();
        }
    }
}
