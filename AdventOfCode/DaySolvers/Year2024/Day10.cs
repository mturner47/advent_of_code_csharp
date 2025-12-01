using Helpers.Helpers;

namespace AdventOfCode.DaySolvers.Year2024
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(c => c - '0').ToList()).ToList();
            var trailHeads = GetTrailheadPositions(grid).ToList();
            var expectedResult = 786;
            var result = trailHeads.Sum(th => GetTrailheadScore(grid, th));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(c => c - '0').ToList()).ToList();
            var trailHeads = GetTrailheadPositions(grid).ToList();
            var expectedResult = 1722;
            var result = trailHeads.Sum(th => GetTrailheadRating(grid, th));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static IEnumerable<(int x, int y)> GetTrailheadPositions(List<List<int>> grid)
        {
            for (var y = 0; y < grid.Count; y++)
            {
                var row = grid[y];
                for (var x = 0; x < row.Count; x++)
                {
                    if (row[x] == 0)
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        private static int GetTrailheadScore(List<List<int>> grid, (int x, int y) trailhead)
        {
            var pointsToCheck = new Queue<(int x, int y)>();
            pointsToCheck.Enqueue(trailhead);
            var seenLocations = new HashSet<(int x, int y)> { trailhead };
            var nineCount = 0;
            while (pointsToCheck.Count > 0)
            {
                var (x, y) = pointsToCheck.Dequeue();
                var num = grid[y][x];
                if (num == 9)
                {
                    nineCount++;
                    continue;
                }
                foreach (var adjacentPoint in DirectionExtensions.GetAllMovements((x, y)))
                {
                    var (aX, aY) = adjacentPoint;
                    if (seenLocations.Contains(adjacentPoint)) continue;
                    if (aX < 0 || aX >= grid[0].Count || aY < 0 || aY >= grid.Count) continue;
                    if (grid[aY][aX] != num + 1) continue;
                    seenLocations.Add(adjacentPoint);
                    pointsToCheck.Enqueue(adjacentPoint);
                }
            }
            return nineCount;
        }

        private static int GetTrailheadRating(List<List<int>> grid, (int x, int y) trailhead)
        {
            var pointsToCheck = new Queue<(int x, int y)>();
            pointsToCheck.Enqueue(trailhead);
            var nineCount = 0;
            while (pointsToCheck.Count > 0)
            {
                var (x, y) = pointsToCheck.Dequeue();
                var num = grid[y][x];
                if (num == 9)
                {
                    nineCount++;
                    continue;
                }
                foreach (var adjacentPoint in DirectionExtensions.GetAllMovements((x, y)))
                {
                    var (aX, aY) = adjacentPoint;
                    if (aX < 0 || aX >= grid[0].Count || aY < 0 || aY >= grid.Count) continue;
                    if (grid[aY][aX] != num + 1) continue;
                    pointsToCheck.Enqueue(adjacentPoint);
                }
            }
            return nineCount;
        }

    }
}
