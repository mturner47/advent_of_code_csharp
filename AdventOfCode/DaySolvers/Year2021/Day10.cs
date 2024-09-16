namespace AdventOfCode.Year2021
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.Sum(GetPointValueForCorruptedLine);
        }

        public object HardSolution(IList<string> lines)
        {
            var incompleteLineScores = lines.Select(GetPointValueForIncompleteLine).Where(d => d != 0).OrderBy(d => d).ToList();
            return incompleteLineScores[incompleteLineScores.Count / 2];
        }

        private static double GetPointValueForCorruptedLine(string line)
        {
            var openingChars = new List<char> { '(', '[', '{', '<' };

            var closingChars = new Dictionary<char, (char, double)>
            {
                { ')', ('(', 3) },
                { ']', ('[', 57) },
                { '}', ('{', 1197) },
                { '>', ('<', 25137) },
            };

            var openBrackets = new Stack<char>();
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (openingChars.Contains(c))
                {
                    openBrackets.Push(c);
                }
                else if (closingChars.ContainsKey(c))
                {
                    var latestOpenBracket = openBrackets.Pop();
                    (var openingChar, var pointValue) = closingChars[c];
                    if (latestOpenBracket != openingChar)
                    {
                        return pointValue;
                    }
                }
                else
                {
                    throw new Exception("Bad character - " + c);
                }
            }
            return 0;
        }

        private static double GetPointValueForIncompleteLine(string line)
        {
            var openingChars = new Dictionary<char, double>
            {
                { '(', 1 },
                { '[', 2 },
                { '{', 3 },
                { '<', 4 },
            };

            var closingChars = new Dictionary<char, char>
            {
                { ')', '(' },
                { ']', '[' },
                { '}', '{' },
                { '>', '<' },
            };

            var openBrackets = new Stack<char>();
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (openingChars.ContainsKey(c))
                {
                    openBrackets.Push(c);
                }
                else if (closingChars.ContainsKey(c))
                {
                    var latestOpenBracket = openBrackets.Pop();
                    var openingChar = closingChars[c];
                    if (latestOpenBracket != openingChar)
                    {
                        return 0;
                    }
                }
                else
                {
                    throw new Exception("Bad character - " + c);
                }
            }

            var score = 0d;
            while (openBrackets.Any())
            {
                var pointValue = openingChars[openBrackets.Pop()];
                score = (score * 5) + pointValue;
            }

            return score;
        }
    }
}
