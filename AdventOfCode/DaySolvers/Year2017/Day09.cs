namespace AdventOfCode.Year2017
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (line, _) = RemoveAllGarbage(lines[0]);
            var expectedResult = -1;
            var result = CalculateScore(line);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var (_, amountRemoved) = RemoveAllGarbage(lines[0]);
            var expectedResult = -1;
            var result = amountRemoved;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (string, int) RemoveAllGarbage(string line)
        {
            var totalRemoved = 0;
            while (line.Contains('<'))
            {
                (line, var amountRemoved) = RemoveGarbage(line);
                totalRemoved += amountRemoved;
            }
            return (line, totalRemoved);
        }

        private static (string, int) RemoveGarbage(string line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '<')
                {
                    var amountRemoved = 0;
                    for (var j = i + 1; j < line.Length; j++)
                    {
                        var nextC = line[j];
                        if (nextC == '>') return (line[0..i] + line[(j + 1)..], amountRemoved);
                        if (nextC == '!') j++;
                        else amountRemoved++;
                    }
                }
            }
            throw new NotImplementedException();
        }

        private static int CalculateScore(string line)
        {
            var depth = 0;
            var sum = 0;
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '{')
                {
                    depth += 1;
                    sum += depth;
                }
                else if (c == '}') depth -= 1;
            }
            return sum;
        }
    }
}
