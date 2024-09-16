using System.Runtime.InteropServices;

namespace AdventOfCode.Year2016
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 105;
            var result = lines.Count(SupportsTls);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 258;
            var result = lines.Count(SupportsSsl);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static bool SupportsTls(string line)
        {
            var insideBrackets = false;
            var foundAbbaOutside = false;
            for (var i = 0; i < line.Length - 3; i++)
            {
                if (line[i] == '[')
                {
                    insideBrackets = true;
                    continue;
                }

                if (line[i] == ']' && insideBrackets)
                {
                    insideBrackets = false;
                    continue;
                }

                if (line[i] == line[i + 3] && line[i + 1] == line[i + 2] && line[i] != line[i+1])
                {
                    if (insideBrackets) return false;
                    foundAbbaOutside = true;
                }
            }
            return foundAbbaOutside;
        }

        private static bool SupportsSsl(string line)
        {
            var insideBracketGroups = new List<(char a, char b)>();
            var outsideBracketGroups = new List<(char a, char b)>();
            var insideBrackets = false;
            for (var i = 0; i < line.Length - 2; i++)
            {
                if (line[i] == '[')
                {
                    insideBrackets = true;
                    continue;
                }

                if (line[i] == ']' && insideBrackets)
                {
                    insideBrackets = false;
                    continue;
                }

                if (line[i] == line[i + 2] && line[i] != line[i + 1])
                {
                    var group = (line[i], line[i + 1]);
                    var oppositeGroup = (line[i + 1], line[i]);
                    if (insideBrackets && outsideBracketGroups.Contains(oppositeGroup)) return true;
                    if (!insideBrackets && insideBracketGroups.Contains(oppositeGroup)) return true;

                    if (insideBrackets) insideBracketGroups.Add(group);
                    else outsideBracketGroups.Add(group);
                }
            }
            return false;
        }
    }
}
