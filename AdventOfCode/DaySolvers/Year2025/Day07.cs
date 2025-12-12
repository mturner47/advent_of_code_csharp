using System.Text;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var splitCount = 0;
            var priorLine = lines[0];
            for (var y = 1; y < lines.Count - 1; y++)
            {
                var line = lines[y];

                var sb = new StringBuilder();
                for (var x = 0; x < line.Length; x++)
                {
                    if (x > 0 && priorLine[x - 1] == 'S' && line[x - 1] == '^')
                    {
                        sb.Append('S');
                        continue;
                    }

                    if (x < line.Length - 1 && priorLine[x + 1] == 'S' && line[x+1] == '^')
                    {
                        sb.Append('S');
                        continue;
                    }

                    if (priorLine[x] == 'S' && line[x] == '.')
                    {
                        sb.Append('S');
                        continue;
                    }

                    if (priorLine[x] == 'S' && line[x] == '^')
                    {
                        splitCount++;
                    }
                    sb.Append('.');
                }
                priorLine = sb.ToString();
            }

            var expectedResult = 1609;
            var result = splitCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var priorLine = lines[0].Select(c => c == 'S' ? 1d : 0d).ToArray();

            for (var y = 1; y < lines.Count; y++)
            {
                var line = lines[y];

                var newLine = new double[priorLine.Length];
                for (var x = 0; x < line.Length; x++)
                {
                    var currentCell = 0d;
                    if (x > 0 && priorLine[x - 1] > 0 && line[x - 1] == '^')
                    {
                        currentCell += priorLine[x - 1];
                    }

                    if (x < line.Length - 1 && priorLine[x + 1] > 0 && line[x + 1] == '^')
                    {
                        currentCell += priorLine[x + 1];
                    }

                    if (priorLine[x] > 0 && line[x] == '.')
                    {
                        currentCell += priorLine[x];
                    }

                    if (priorLine[x] == 'S' && line[x] == '^')
                    {
                        currentCell = 0;
                    }
                    newLine[x] = currentCell;
                }
                priorLine = newLine;
            }

            var expectedResult = 12472142047197d;
            var result = priorLine.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
