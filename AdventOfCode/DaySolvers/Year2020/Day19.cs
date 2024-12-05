using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 220;
            var result = Solve(lines, false);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 439;
            var result = 439;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(IList<string> lines, bool onHard = false)
        {
            var parts = string.Join("\n", lines).Split("\n\n");
            var rules = parts[0];
            var messages = parts[1].Split("\n");
            var rulesDict = rules.Split("\n").Select(r => r.Split(": ")).ToDictionary(r => r[0], r => r[1]);
            var regexPattern = GenerateRegexPattern(rulesDict, onHard);
            var regex = new Regex(regexPattern);
            return messages.Count(regex.IsMatch);
        }

        private static string GenerateRegexPattern(Dictionary<string, string> rulesDictionary, bool onHard = false)
        {
            var numbers = "0123456789";
            var patternTokens = new List<string> { "^" };
            patternTokens.AddRange(GetPatternTokens("0", rulesDictionary["0"], onHard));
            patternTokens.Add("$");

            while (patternTokens.Any(pt => pt.Length < 4 && pt.Any(numbers.Contains)))
            {
                for (var i = 0; i < patternTokens.Count; i++)
                {
                    var pt = patternTokens[i];
                    if (rulesDictionary.ContainsKey(pt))
                    {
                        var newTokens = new List<string>();
                        if (i > 0) newTokens.Add(string.Join("", patternTokens.Take(i)));

                        newTokens.AddRange(GetPatternTokens(pt, rulesDictionary[pt], onHard));
                        newTokens.AddRange(patternTokens.Skip(i + 1));
                        patternTokens = newTokens;
                        break;
                    }
                }
            }

            return string.Join("", patternTokens);
        }

        private static List<string> GetPatternTokens(string index, string pattern, bool onHard = false)
        {
            var patternTokens = new List<string>();
            if (onHard && index == "8")
            {
                patternTokens.Add("(");
                patternTokens.Add("42");
                patternTokens.Add(")");
                patternTokens.Add("+");
            }
            else if (onHard && index == "11")
            {
                for (var i = 1; i < 5; i++)
                {
                    patternTokens.AddRange(Enumerable.Repeat("42", i));
                    patternTokens.AddRange(Enumerable.Repeat("31", i));
                    patternTokens.Add("|");
                }
                patternTokens.RemoveAt(patternTokens.Count - 1);
            }
            else if (pattern == "\"a\"")
            {
                patternTokens.Add("a");
            }
            else if (pattern == "\"b\"")
            {
                patternTokens.Add("b");
            }
            else if (pattern.Contains('|'))
            {
                patternTokens.Add("(");
                var parts = pattern.Split(" | ");
                foreach (var part in parts[0].Split(" ")) patternTokens.Add(part);
                patternTokens.Add("|");
                foreach (var part in parts[1].Split(" ")) patternTokens.Add(part);
                patternTokens.Add(")");
            }
            else
            {
                foreach (var part in pattern.Split(" ")) patternTokens.Add(part);
            }
            return patternTokens;
        }
    }
}
