using AdventOfCode.DaySolvers.Year2017;
using Helpers.Helpers;

namespace AdventOfCode.Year2017
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 8292;
            var result = GetHashes(lines[0]).Sum(h => h.Count(c => c == '1'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var hashes = GetHashes(lines[0]);

            var groupCount = 0;
            var seenCoordinates = new List<(int x, int y)>();
            for (var y = 0; y < hashes.Count; y++)
            {
                var hash = hashes[y];
                for (var x = 0; x < hash.Length; x++)
                {
                    if (hash[x] == '1' && !seenCoordinates.Contains((x, y)))
                    {
                        groupCount++;
                        seenCoordinates.Add((x, y));
                        var cellsToCheck = new Queue<(int x, int y)>();
                        cellsToCheck.Enqueue((x, y));
                        while (cellsToCheck.Count > 0)
                        {
                            var cellToCheck = cellsToCheck.Dequeue();
                            var surroundingCells = DirectionExtensions.GetAllMovements(cellToCheck);
                            foreach (var (cx, cy) in surroundingCells)
                            {
                                if (cx < 0 || cy < 0 || cx >= hash.Length || cy >= hashes.Count) continue;
                                if (hashes[cy][cx] == '1' && !seenCoordinates.Contains((cx, cy)))
                                {
                                    seenCoordinates.Add((cx, cy));
                                    cellsToCheck.Enqueue((cx, cy));
                                }
                            }
                        }
                    }
                }
            }

            var expectedResult = 1069;
            var result = groupCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private List<string> GetHashes(string input)
        {
            return Enumerable.Range(0, 128)
                .Select(i => $"{input}-{i}")
                .Select(Shared2017.KnotHash)
                .Select(MathHelpers.ConvertHexStringToBitString)
                .ToList();
        }
    }
}
