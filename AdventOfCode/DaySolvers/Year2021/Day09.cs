namespace AdventOfCode.Year2021
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var map = lines.Select(l => l.AsEnumerable().Select(char.GetNumericValue).ToList()).ToList();
            var sum = 0d;
            for (var x = 0; x < map.Count; x++)
            {
                var row = map[x];
                for (var y = 0; y < row.Count; y++)
                {
                    var value = row[y];
                    if (y > 0 && row[y - 1] <= value) continue;
                    if (x > 0 && map[x - 1][y] <= value) continue;
                    if (y < row.Count - 1 && row[y + 1] <= value) continue;
                    if (x < map.Count - 1 && map[x + 1][y] <= value) continue;
                    sum += value + 1;
                }
            }
            return sum;
        }

        public object HardSolution(IList<string> lines)
        {
            var map = lines.Select(l => l.AsEnumerable().Select(char.GetNumericValue).ToList()).ToList();
            var basinSizes = new List<double>();

            for (var x = 0; x < map.Count; x++)
            {
                var row = map[x];
                for (var y = 0; y < row.Count; y++)
                {
                    var value = row[y];
                    if (y > 0 && row[y - 1] <= value) continue;
                    if (x > 0 && map[x - 1][y] <= value) continue;
                    if (y < row.Count - 1 && row[y + 1] <= value) continue;
                    if (x < map.Count - 1 && map[x + 1][y] <= value) continue;
                    basinSizes.Add(CalculateBasinSize(map, x, y));
                }
            }

            return basinSizes.OrderByDescending(x => x).Take(3).Aggregate(1d, (a, b) => a * b);
        }

        private static double CalculateBasinSize(List<List<double>> map, int x, int y)
        {
            var mapOfBasin = map.Select(r => r.Select(c => false).ToList()).ToList();
            var maxX = mapOfBasin.Count;
            var maxY = mapOfBasin[0].Count;
            var pointsToCheck = new Stack<(int, int)>();
            pointsToCheck.Push((x, y));
            mapOfBasin[x][y] = true;
            while (pointsToCheck.Count > 0)
            {
                var pointToCheck = pointsToCheck.Pop();
                (x, y) = pointToCheck;
                var localPoints = new List<(int, int)> { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1) };
                foreach (var localPoint in localPoints)
                {
                    (var localX, var localY) = localPoint;
                    if (localX >= 0
                        && localY >= 0
                        && localX < maxX
                        && localY < maxY
                        && !mapOfBasin[localX][localY]
                        && map[localX][localY] != 9)
                    {
                        mapOfBasin[localX][localY] = true;
                        pointsToCheck.Push((localX, localY));
                    }
                }
            }

            return mapOfBasin.Sum(r => r.Count(c => c));
        }
    }
}
