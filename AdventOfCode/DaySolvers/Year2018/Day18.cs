using Helpers.Helpers;
using System.Text;

namespace AdventOfCode.Year2018
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            for (var i = 0; i < 10; i++)
            {
                lines = RunMinute(lines);
            }

            var treeCount = lines.Sum(l => l.Count(c => c == '|'));
            var lumberyardCount = lines.Sum(l => l.Count(c => c == '#'));
            var expectedResult = 360720;
            var result = treeCount*lumberyardCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var seenPatterns = new List<string> { string.Join(",", lines) };
            var firstIndex = 0;
            var secondIndex = 0;
            var loopPattern = new List<string>();
            var target = 1_000_000_000;

            for (var i = 1; i <= target; i++)
            {
                lines = RunMinute(lines);
                var pattern = string.Join(",", lines);

                if (seenPatterns.Contains(pattern))
                {
                    firstIndex = seenPatterns.IndexOf(pattern);
                    secondIndex = i;
                    loopPattern = lines.ToList();
                    break;
                }
                else seenPatterns.Add(pattern);
            }

            var loopSize = secondIndex - firstIndex;
            var numLoopsToAdd = (target - secondIndex) / loopSize;
            var newI = secondIndex + loopSize*numLoopsToAdd;
            var newL = loopPattern;

            for (var i = newI; i < target; i++)
            {
                newL = RunMinute(newL);
            }

            var treeCount = newL.Sum(l => l.Count(c => c == '|'));
            var lumberyardCount = newL.Sum(l => l.Count(c => c == '#'));
            var expectedResult = 197276;
            var result = treeCount * lumberyardCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<string> RunMinute(IList<string> lines)
        {
            var maxX = lines[0].Length;
            var maxY = lines.Count;
            var newLines = new List<string>();
            for (var y = 0; y < maxY; y++)
            {
                var newLine = new StringBuilder();
                for (var x = 0; x < maxX; x++)
                {
                    var surroundingPositions = DirectionExtensions.GetAllMovements((x, y), true);
                    var emptyCount = 0;
                    var treeCount = 0;
                    var lumberyardCount = 0;
                    var current = lines[y][x];
                    var next = current;
                    foreach (var (sx, sy) in surroundingPositions)
                    {
                        if (sx < 0 || sy < 0 || sx >= maxX || sy >= maxY) continue;
                        var c = lines[sy][sx];
                        if (c == '.') emptyCount++;
                        if (c == '|') treeCount++;
                        if (c == '#') lumberyardCount++;
                    }
                    if (current == '.' && treeCount >= 3) next = '|';
                    if (current == '|' && lumberyardCount >= 3) next = '#';
                    if (current == '#' && (lumberyardCount == 0 || treeCount == 0)) next = '.';
                    newLine.Append(next);
                }
                newLines.Add(newLine.ToString());
            }
            return newLines;
        }
    }
}
