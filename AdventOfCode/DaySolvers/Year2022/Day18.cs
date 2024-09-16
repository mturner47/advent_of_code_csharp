namespace AdventOfCode.Year2022
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var points = lines.Select(ParseLine).ToList();
            return points.Select(p => 6 - ConnectedSides(p, points)).Sum();
        }

        public object HardSolution(IList<string> lines)
        {
            var points = lines.Select(ParseLine).ToList();
            var upperBoundX = points.Select(p => p.x).Max() + 1;
            var lowerBoundX = points.Select(p => p.x).Min() - 1;
            var upperBoundY = points.Select(p => p.y).Max() + 1;
            var lowerBoundY = points.Select(p => p.y).Min() - 1;
            var upperBoundZ = points.Select(p => p.z).Max() + 1;
            var lowerBoundZ = points.Select(p => p.z).Min() - 1;

            var airPoints = new List<(int x, int y, int z)>();
            var pointsToSearch = new List<(int x, int y, int z)> { (lowerBoundX, lowerBoundY, lowerBoundZ) };

            var moves = new List<(int x, int y, int z)> { (-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1) };

            while (pointsToSearch.Any())
            {
                var pointToSearch = pointsToSearch.First();
                airPoints.Add((pointToSearch));
                pointsToSearch = pointsToSearch.Skip(1).ToList();
                var (aX, aY, aZ) = pointToSearch;
                foreach (var move in moves)
                {
                    var pointToCheck = (aX + move.x, aY + move.y, aZ + move.z);
                    var (ptcX, ptcY, ptcZ) = pointToCheck;
                    if (ptcX < lowerBoundX || ptcX > upperBoundX) continue;
                    if (ptcY < lowerBoundY || ptcY > upperBoundY) continue;
                    if (ptcZ < lowerBoundZ || ptcZ > upperBoundZ) continue;
                    if (points.Contains(pointToCheck)) continue;
                    if (airPoints.Contains(pointToCheck)) continue;
                    if (pointsToSearch.Contains(pointToCheck)) continue;
                    pointsToSearch.Add(pointToCheck);
                }
            }

            return points.Select(p => ConnectedSides(p, airPoints)).Sum();
        }

        public static (int x, int y, int z) ParseLine(string line)
        {
            var parts = line.Split(",").Select(int.Parse).ToList();
            return (parts[0], parts[1], parts[2]);
        }

        public static int ConnectedSides((int, int, int) point, List<(int x, int y, int z)> points)
        {
            var (x, y, z) = point;
            return points.Count(p =>
            {
                return (p.x == x && p.y == y && Math.Abs(p.z - z) == 1)
                    || (p.x == x && p.z == z && Math.Abs(p.y - y) == 1)
                    || (p.y == y && p.z == z && Math.Abs(p.x - x) == 1);
            });
        }
    }
}
