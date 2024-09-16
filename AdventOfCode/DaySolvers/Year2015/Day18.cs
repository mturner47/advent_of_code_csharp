using Helpers.Helpers;
using System.Text;

namespace AdventOfCode.Year2015
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var currentLines = lines.ToList();
            var nextLines = new List<string>();
            var maxX = lines[0].Length - 1;
            var maxY = lines.Count - 1;

            for (var t = 0; t < 100; t++)
            {
                nextLines = [];
                for (var y = 0; y < currentLines.Count; y++)
                {
                    var sb = new StringBuilder();
                    var line = currentLines[y];
                    for (var x = 0; x < line.Length; x++)
                    {
                        var numOn = 0;
                        var currentC = line[x];
                        foreach (var (adjX, adjY) in DirectionExtensions.GetAllMovements((x, y), true))
                        {
                            if (adjX < 0 || adjY < 0 || adjX > maxX || adjY > maxY) continue;
                            if (currentLines[adjY][adjX] == '#') numOn++;
                        }
                        var nextC = currentC;
                        if (currentC == '#' && numOn != 2 && numOn != 3) nextC = '.';
                        if (currentC == '.' && numOn == 3) nextC = '#';
                        sb.Append(nextC);
                    }
                    nextLines.Add(sb.ToString());
                }
                currentLines = nextLines;
            }

            var expectedResult = 768;
            var result = currentLines.Sum(l => l.Count(c => c == '#'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var currentLines = lines.ToList();
            var nextLines = new List<string>();
            var maxX = lines[0].Length - 1;
            var maxY = lines.Count - 1;

            for (var t = 0; t < 100; t++)
            {
                nextLines = [];
                for (var y = 0; y < currentLines.Count; y++)
                {
                    var sb = new StringBuilder();
                    var line = currentLines[y];
                    for (var x = 0; x < line.Length; x++)
                    {
                        if ((x == 0 || x == maxX) && (y == 0 || y == maxY))
                        {
                            sb.Append('#');
                            continue;
                        }

                        var numOn = 0;
                        var currentC = line[x];
                        foreach (var (adjX, adjY) in DirectionExtensions.GetAllMovements((x, y), true))
                        {
                            if (adjX < 0 || adjY < 0 || adjX > maxX || adjY > maxY) continue;
                            if (currentLines[adjY][adjX] == '#') numOn++;
                        }
                        var nextC = currentC;
                        if (currentC == '#' && numOn != 2 && numOn != 3) nextC = '.';
                        if (currentC == '.' && numOn == 3) nextC = '#';
                        sb.Append(nextC);
                    }
                    nextLines.Add(sb.ToString());
                }
                currentLines = nextLines;
            }

            var expectedResult = 781;
            var result = currentLines.Sum(l => l.Count(c => c == '#'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
