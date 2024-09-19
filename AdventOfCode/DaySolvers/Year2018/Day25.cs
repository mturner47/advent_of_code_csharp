namespace AdventOfCode.Year2018
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var points = lines.Select(Parse).ToList();

            var constellationCount = 0;
            while (points.Count > 0)
            {
                var currentPoint = points.First();
                points.Remove(currentPoint);
                var pointsInCurrentConstellation = new HashSet<(int x, int y, int z, int t)> { currentPoint };

                while (true)
                {
                    var connectedPoints = points.Where(p => pointsInCurrentConstellation.Any(p2 => Connects(p, p2))).ToList();
                    if (connectedPoints.Count == 0) break;
                    foreach (var cp in connectedPoints)
                    {
                        pointsInCurrentConstellation.Add(cp);
                        points.Remove(cp);
                    }
                }
                constellationCount++;
            }

            var expectedResult = 370;
            var result = constellationCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static bool Connects((int x, int y, int z, int t) a, (int x, int y, int z, int t) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) + Math.Abs(a.t - b.t) <= 3;
        }

        private static (int x, int y, int z, int t) Parse(string s)
        {
            var parts = s.Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
        }
    }
}
