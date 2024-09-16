using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var asteroids = new List<(int x, int y)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') asteroids.Add((x, y));
                }
            }

            var bestCount = 0;
            var bestAsteroid = (x:0, y:0);
            List<(int x, int y, double degrees)> bestSeenAsteroids = [];
            foreach (var asteroid in asteroids)
            {
                var seenAsteroids = FindVisibleAsteroids(asteroids, asteroid);
                var count = seenAsteroids.Count;
                if (bestCount < count)
                {
                    bestCount = count;
                    bestAsteroid = asteroid;
                    bestSeenAsteroids = seenAsteroids;
                }
            }

            return $"{bestCount} at ({bestAsteroid.x}, {bestAsteroid.y})";
        }

        public object HardSolution(IList<string> lines)
        {
            var asteroids = new List<(int x, int y)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') asteroids.Add((x, y));
                }
            }

            var bestCount = 0;
            var bestAsteroid = (x: 0, y: 0);
            List<(int x, int y, double degrees)> bestSeenAsteroids = [];
            foreach (var asteroid in asteroids)
            {
                var seenAsteroids = FindVisibleAsteroids(asteroids, asteroid);
                var count = seenAsteroids.Count;
                if (bestCount < count)
                {
                    bestCount = count;
                    bestAsteroid = asteroid;
                    bestSeenAsteroids = seenAsteroids;
                }
            }

            var numToDestroy = 200;
            while (bestSeenAsteroids.Count < numToDestroy)
            {
                numToDestroy -= bestSeenAsteroids.Count;
                foreach (var (ax, ay, _) in bestSeenAsteroids)
                {
                    asteroids.Remove((ax, ay));
                }

                bestSeenAsteroids = FindVisibleAsteroids(asteroids, bestAsteroid);
            }

            var (lx, ly, _) = bestSeenAsteroids.OrderBy(a => a.degrees).Skip(numToDestroy - 1).First();
            return lx*100 + ly;
        }

        private static List<(int x, int y, double degrees)> FindVisibleAsteroids(List<(int x, int y)> asteroids, (int x, int y) asteroid)
        {
            var count = 0;
            var (x1, y1) = asteroid;
            var seenAsteroids = new List<(int x, int y, double degrees)>();
            foreach (var asteroid2 in asteroids)
            {
                if (asteroid2 == asteroid) continue;
                var distance2 = MathHelpers.ManhattanDistance(asteroid, asteroid2);
                var (x2, y2) = asteroid2;
                var slope2 = x2 == x1 ? (double?)null : ((double)(y2 - y1)) / (x2 - x1);

                var isSeen = true;
                foreach (var asteroid3 in asteroids)
                {
                    if (asteroid3 == asteroid || asteroid3 == asteroid2) continue;
                    var distance3 = MathHelpers.ManhattanDistance(asteroid, asteroid3);
                    var (x3, y3) = asteroid3;
                    var slope3 = x3 == x1 ? (double?)null : ((double)(y3 - y1)) / (x3 - x1);

                    if (slope3 == slope2
                        && distance3 < distance2
                        && Math.Sign(x1 - x2) == Math.Sign(x1 - x3)
                        && Math.Sign(y1 - y2) == Math.Sign(y1 - y3))
                    {
                        isSeen = false;
                        break;
                    }
                }

                if (isSeen)
                {
                    double degrees = 0;
                    var isUp = y2 < y1;
                    var isRight = x2 > x1;
                    if (!slope2.HasValue)
                    {
                        degrees = (y2 < y1) ? 0 : 180;
                    }
                    else
                    {
                        degrees = Math.Atan(Math.Abs(slope2.Value)) * 180 / Math.PI;
                        if (isUp && isRight) degrees = 90 - degrees;
                        if (!isUp && isRight) degrees = 90 + degrees;
                        if (!isUp && !isRight) degrees = 270 - degrees;
                        if (isUp && !isRight) degrees = 270 + degrees;
                    }

                    seenAsteroids.Add((x2, y2, degrees));
                    count++;
                }
            }

            return seenAsteroids;
        }
    }
}
