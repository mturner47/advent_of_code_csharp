using System.Text;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var freeSpaces = 0;

            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var countRolls = 0;
                    if (lines[y][x] != '@') continue;
                    for (var xDelta = -1; xDelta <= 1; xDelta++)
                    {
                        for (var yDelta = -1; yDelta <= 1; yDelta++)
                        {
                            var xInner = x + xDelta;
                            var yInner = y + yDelta;
                            if (xInner == x && yInner == y) continue;
                            if (xInner < 0 || xInner >= lines[y].Length || yInner < 0 || yInner >= lines.Count) continue;
                            if (lines[yInner][xInner] == '@') countRolls++;
                        }
                    }
                    if (countRolls < 4) freeSpaces++;
                }
            }

            var expectedResult = 1564;
            var result = freeSpaces;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var spaces = lines.Select(l => l.Select(c => c == '@').ToArray()).ToArray();
            var totalRemoved = 0;
            int countRemoved;
            do
            {
                countRemoved = 0;
                for (var y = 0; y < spaces.Length; y++)
                {
                    for (var x = 0; x < spaces[y].Length; x++)
                    {
                        var countAdjacent = 0;
                        if (!spaces[y][x]) continue;
                        for (var xDelta = -1; xDelta <= 1; xDelta++)
                        {
                            for (var yDelta = -1; yDelta <= 1; yDelta++)
                            {
                                var xInner = x + xDelta;
                                var yInner = y + yDelta;
                                if (xInner == x && yInner == y) continue;
                                if (xInner < 0 || xInner >= spaces[y].Length || yInner < 0 || yInner >= spaces.Length) continue;
                                if (spaces[yInner][xInner]) countAdjacent++;
                            }
                        }

                        if (countAdjacent < 4)
                        {
                            spaces[y][x] = false;
                            countRemoved++;
                        }
                    }
                }
                totalRemoved += countRemoved;

            } while (countRemoved > 0);
            var expectedResult = 9401;
            var result = totalRemoved;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
