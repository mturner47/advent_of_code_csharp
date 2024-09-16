using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var codeCharCount = lines.Sum(l => l.Length);
            var memCharCount = lines.Sum(l => Regex.Unescape(l[1..^1]).Length);

            var expectedResult = 1371;
            var result = codeCharCount - memCharCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var codeCharCount = lines.Sum(l => l.Length);
            var encodedLines = lines.Select(l => "\"" + l.Replace(@"\", @"\\").Replace(@"""", @"\""") + "\"");
            var encodedCodeCount = encodedLines.Sum(l => l.Length);
            var expectedResult = 2117;
            var result = encodedCodeCount - codeCharCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
