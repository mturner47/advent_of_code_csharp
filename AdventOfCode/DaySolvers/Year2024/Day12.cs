using Helpers.Helpers;

namespace AdventOfCode.DaySolvers.Year2024
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var regions = GetRegions(lines);
            var expectedResult = 1421958;
            var result = regions.Sum(r => GetPriceEasy(r, lines[0].Length, lines.Count));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var regions = GetRegions(lines);
            var expectedResult = 885394;
            var result = regions.Sum(r => GetPriceHard(r, lines[0].Length, lines.Count));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<HashSet<(int x, int y)>> GetRegions(IList<string> lines)
        {
            var unexploredPoints = new HashSet<(int x, int y)>();
            var regions = new List<HashSet<(int x, int y)>>();
            var maxX = lines[0].Length;
            var maxY = lines.Count;
            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines.Count; x++)
                {
                    unexploredPoints.Add((x, y));
                }
            }

            while (unexploredPoints.Count != 0)
            {
                var pointToExplore = unexploredPoints.First();
                var (x, y) = pointToExplore;
                unexploredPoints.Remove(pointToExplore);
                var c = lines[y][x];
                var region = new HashSet<(int x, int y)> { pointToExplore };
                var pointsToFollow = new Queue<(int x, int y)>();
                pointsToFollow.Enqueue(pointToExplore);
                while (pointsToFollow.Count > 0)
                {
                    var pointToFollow = pointsToFollow.Dequeue();
                    foreach (var point in DirectionExtensions.GetAllMovements(pointToFollow))
                    {
                        if (point.x < 0 || point.y < 0 || point.x >= maxX || point.y >= maxY) continue;
                        var c2 = lines[point.y][point.x];
                        if (c != c2) continue;
                        if (region.Contains(point)) continue;
                        region.Add(point);
                        unexploredPoints.Remove(point);
                        pointsToFollow.Enqueue(point);
                    }
                }
                regions.Add(region);
            }

            return regions;
        }

        private static double GetPriceEasy(HashSet<(int x, int y)> region, int maxX, int maxY)
        {
            var area = region.Count;
            var perimeter = 0d;
            foreach (var point in region)
            {
                foreach (var adjacentPoint in DirectionExtensions.GetAllMovements(point))
                {
                    if (!region.Contains(adjacentPoint)) perimeter++;
                }
            }
            return area*perimeter;
        }

        private static double GetPriceHard(HashSet<(int x, int y)> region, int maxX, int maxY)
        {
            var area = region.Count;
            var wallsToCheck = new HashSet<((int x, int y) point, Direction direction)>();
            foreach (var point in region)
            {
                foreach (var direction in DirectionExtensions.EnumerateDirections())
                {
                    var adjacentPoint = direction.GetMovement(point);
                    if (!region.Contains(adjacentPoint)) wallsToCheck.Add((point, direction));
                }
            }

            var numWalls = 0d;
            while (wallsToCheck.Count > 0)
            {
                var wall = wallsToCheck.First();
                wallsToCheck.Remove(wall);
                var newWall = new HashSet<(int x, int y)> { wall.point };
                numWalls++;
                var foundWall = true;
                while (foundWall && wallsToCheck.Count > 0)
                {
                    foundWall = false;
                    foreach (var (point, direction) in wallsToCheck.Where(w => w.direction == wall.direction))
                    {
                        foreach (var wp in newWall)
                        {
                            if (DirectionExtensions.IsAdjacent(wp, point))
                            {
                                wallsToCheck.Remove((point, direction));
                                newWall.Add(point);
                                foundWall = true;
                                break;
                            }
                        }
                    }
                }
            }

            return area * numWalls;
        }
    }
}
