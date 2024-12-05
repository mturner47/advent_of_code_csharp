using System.Text;

namespace AdventOfCode.Year2020
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 14006719520523;
            var result = lines.Select(SolveEasy).Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 545115449981968;
            var result = lines.Select(SolveHard).Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static long SolveEasy(string line)
        {
            line = line.Replace(" ", "");
            var stack = new Stack<(long? value, char priorOperation)>();
            var priorOperation = 'N';
            long currentValue = 0;
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if ("0123456789".Contains(c))
                {
                    var val = int.Parse(c.ToString());
                    if (priorOperation == 'N') currentValue = val;
                    else if (priorOperation == '+') currentValue += val;
                    else currentValue *= val;
                }
                else if (c == '*') priorOperation = '*';
                else if (c == '+') priorOperation = '+';
                else if (c == '(')
                {
                    stack.Push((currentValue, priorOperation));
                    priorOperation = 'N';
                }
                else if (c == ')')
                {
                    var val = currentValue;
                    (var priorValue, priorOperation) = stack.Pop();
                    if (priorOperation == 'N') currentValue = val;
                    else if (priorOperation == '+') currentValue = (priorValue ?? 0) + val;
                    else currentValue = (priorValue ?? 1) * val;
                }
            }
            return currentValue;
        }

        private static long SolveHard(string line)
        {
            const string numbers = "0123456789";
            var tokens = line.Replace(" ", "").Select(c => (op: numbers.Contains(c) ? 'N' : c, val: numbers.Contains(c) ? long.Parse(c.ToString()) : 0L)).ToList();
            while (tokens.Count > 1)
            {
                var newTokens = new List<(char op, long val)>();
                var madeSubstitution = false;
                for (var i = 0; i < tokens.Count - 2; i++)
                {
                    if (tokens[i].op == 'N' && tokens[i + 1].op == '+' && tokens[i + 2].op == 'N')
                    {
                        newTokens = tokens.Take(i).ToList();
                        newTokens.Add(('N', tokens[i].val + tokens[i + 2].val));
                        newTokens.AddRange(tokens.Skip(i + 3));
                        tokens = newTokens;
                        madeSubstitution = true;
                        break;
                    }
                }
                if (madeSubstitution) continue;

                for (var i = 0; i < tokens.Count - 2; i++)
                {
                    if (tokens[i].op == '(' && tokens[i + 1].op == 'N' && tokens[i + 2].op == ')')
                    {
                        newTokens = tokens.Take(i).ToList();
                        newTokens.Add(tokens[i + 1]);
                        newTokens.AddRange(tokens.Skip(i + 3));
                        tokens = newTokens;
                        madeSubstitution = true;
                        break;
                    }
                }
                if (madeSubstitution) continue;

                for (var i = 0; i < tokens.Count - 4; i++)
                {
                    if (tokens[i].op == '(' && tokens[i + 1].op == 'N' && tokens[i + 2].op == '*' && tokens[i + 3].op == 'N' && tokens[i + 4].op == ')')
                    {
                        newTokens = tokens.Take(i).ToList();
                        newTokens.Add(('N', tokens[i + 1].val * tokens[i + 3].val));
                        newTokens.AddRange(tokens.Skip(i + 5));
                        tokens = newTokens;
                        madeSubstitution = true;
                        break;
                    }
                }
                if (madeSubstitution) continue;

                for (var i = 0; i < tokens.Count - 3; i++)
                {
                    if (tokens[i].op == 'N' && tokens[i + 1].op == '*' && tokens[i + 2].op == 'N' && tokens[i + 3].op != '+')
                    {
                        newTokens = tokens.Take(i).ToList();
                        newTokens.Add(('N', tokens[i].val * tokens[i + 2].val));
                        newTokens.AddRange(tokens.Skip(i + 3));
                        tokens = newTokens;
                        madeSubstitution = true;
                        break;
                    }
                }
                if (madeSubstitution) continue;

                for (var i = 0; i < tokens.Count - 2; i++)
                {
                    if (tokens[i].op == 'N' && tokens[i + 1].op == '*' && tokens[i + 2].op == 'N')
                    {
                        newTokens = tokens.Take(i).ToList();
                        newTokens.Add(('N', tokens[i].val * tokens[i + 2].val));
                        newTokens.AddRange(tokens.Skip(i + 3));
                        tokens = newTokens;
                        madeSubstitution = true;
                        break;
                    }
                }
            }
            return tokens[0].val;
        }
    }
}
