using System.Text;

namespace AdventOfCode.Year2017
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 197;
            var result = Solve(lines, 5);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 3081737;
            var result = Solve(lines, 18);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(IList<string> lines, int numIterations)
        {
            var ruleList = lines.Select(Parse).SelectMany(s => s).ToDictionary(s => s.pattern, s => s.replacement);

            var pattern = ".#./..#/###".Split("/").ToList();

            for (var i = 0; i < numIterations; i++)
            {
                var newPattern = new List<string>();
                if (pattern.Count % 2 == 0)
                {
                    for (var y = 0; y < pattern.Count - 1; y += 2)
                    {
                        for (var n = 0; n < 3; n++) newPattern.Add("");

                        for (var x = 0; x < pattern.Count - 1; x += 2)
                        {
                            var subPattern = pattern[y][x..(x + 2)] + "/" + pattern[y + 1][x..(x + 2)];
                            var replacement = ruleList[subPattern];
                            for (var n = 0; n < 3; n++)
                            {
                                newPattern[(y * 3 / 2) + n] += replacement[n];
                            }
                        }
                    }
                }
                else
                {
                    for (var y = 0; y < pattern.Count - 2; y += 3)
                    {
                        for (var n = 0; n < 4; n++) newPattern.Add("");

                        for (var x = 0; x < pattern.Count - 2; x += 3)
                        {
                            var subPattern = pattern[y][x..(x + 3)] + "/" + pattern[y + 1][x..(x + 3)] + "/" + pattern[y + 2][x..(x + 3)];
                            var replacement = ruleList[subPattern];
                            for (var n = 0; n < 4; n++)
                            {
                                newPattern[(y * 4 / 3) + n] += replacement[n];
                            }
                        }
                    }
                }
                pattern = newPattern;
            }
            return pattern.Sum(pl => pl.Count(c => c == '#'));
        }

        private static List<(string pattern, List<string> replacement)> Parse(string line)
        {
            var parts = line.Split(" => ");

            var inputPattern = parts[0].Split("/").ToList();
            var patternPerms = GetPatternPermuations(inputPattern);
            var outputPattern = parts[1].Split("/").ToList();

            return patternPerms.Select(pp => (pp, outputPattern)).ToList();
        }

        private static List<string> GetPatternPermuations(List<string> pattern)
        {
            var patterns = new List<List<string>> { pattern };

            for (var i = 0; i < 3; i++)
            {
                pattern = RotateCW(pattern);
                patterns.Add(pattern);
            }
            pattern = Flip(pattern);
            patterns.Add(pattern);
            for (var i = 0; i < 3; i++)
            {
                pattern = RotateCW(pattern);
                patterns.Add(pattern);
            }
            return patterns.Select(p => string.Join("/", p)).Distinct().ToList();
        }

        private static void PrintPattern(List<string> pattern)
        {
            foreach (var pl in pattern)
            {
                Console.WriteLine(pl);
            }
            Console.WriteLine();
        }

        private static List<string> RotateCW(List<string> pattern)
        {
            var newPattern = new List<string>();
            for (var j = 0; j < pattern[0].Length; j++)
            {
                var newPatternLine = "";
                for (var i = pattern[0].Length - 1; i >= 0; i--)
                {
                    var c = pattern[i][j];
                    newPatternLine += c;
                }
                newPattern.Add(newPatternLine);
            }
            return newPattern;
        }

        private static List<string> Flip(List<string> pattern)
        {
            return pattern.Select(p => new string(p.Reverse().ToArray())).ToList();
        }
    }
}
