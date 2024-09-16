using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 156366;
            var result = DigitRegex().Matches(lines[0]).Sum(m => int.Parse(m.Groups[1].Value));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var (json, _) = RemoveRedObjects(lines[0]);
            var expectedResult = 96852;
            var result = DigitRegex().Matches(json).Sum(m => int.Parse(m.Groups[1].Value));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (string resultString, int lastIndex) RemoveRedObjects (string json)
        {
            var sb = new StringBuilder(json[0]);
            var openingBracket = json[0];
            var closingBracket = openingBracket == '{' ? '}' : ']';
            for (var i = 1; i < json.Length; i++)
            {
                var c = json[i];
                if (c == closingBracket)
                {
                    sb.Append(c);
                    return (sb.ToString(), i);
                }

                if (c == '{' || c == '[')
                {
                    var (resultString, lastIndex) = RemoveRedObjects(json[i..]);
                    sb.Append(resultString);
                    i += lastIndex;
                    continue;
                }

                if (c == 'r' && i + 2 < json.Length && json[i + 1] == 'e' && json[i + 2] == 'd')
                {
                    if (openingBracket == '{')
                    {
                        var countOpeningBrackets = 0;
                        var currentIndex = i;
                        while (true)
                        {
                            var nextOpeningBracketIndex = json.IndexOf('{', currentIndex);
                            var nextClosingBrackingIndex = json.IndexOf('}', currentIndex);
                            if (nextOpeningBracketIndex != -1 && nextOpeningBracketIndex < nextClosingBrackingIndex)
                            {
                                currentIndex = nextOpeningBracketIndex + 1;
                                countOpeningBrackets++;
                            }
                            else
                            {
                                if (countOpeningBrackets > 0)
                                {
                                    currentIndex = nextClosingBrackingIndex + 1;
                                    countOpeningBrackets--;
                                }
                                else
                                {
                                    return ("", nextClosingBrackingIndex);
                                }
                            }
                        }
                    }
                }
                sb.Append(c);
            }
            return (sb.ToString(), json.Length);
        }

        [GeneratedRegex(@"(-?\d+)")]
        private static partial Regex DigitRegex();
    }
}
