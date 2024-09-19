using Helpers.Helpers;

namespace AdventOfCode.Year2018
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var clayCells = GetClayCells(lines);
            var (settled, seen) = Solve(clayCells);
            var expectedResult = 31038;
            var result = settled + seen;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var clayCells = GetClayCells(lines);
            var (settled, _) = Solve(clayCells);
            var expectedResult = 25250;
            var result = settled;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int settled, int seen) Solve(HashSet<(int x, int y)> claySpots)
        {
            var waterStart = (x:500, y:0);
            var seenWaterSpots = new HashSet<(int x, int y)>();
            var settledWaterSpots = new HashSet<(int x, int y)>();
            var minY = claySpots.Min(p => p.y);
            var maxY = claySpots.Max(p => p.y);
            var deadEnds = new HashSet<(int x, int y)>();
            bool shouldDraw = false;

            while (true)
            {
                var currentPosition = waterStart;
                var latestDropPosition = waterStart;
                if (deadEnds.Contains(waterStart)) break;
                while (true)
                {
                    var downPosition = Direction.South.GetMovement(currentPosition);
                    if (!settledWaterSpots.Contains(downPosition) && !claySpots.Contains(downPosition))
                    {
                        if (downPosition.y > maxY)
                        {
                            deadEnds.Add(latestDropPosition);
                            break;
                        }
                        currentPosition = downPosition;
                        seenWaterSpots.Add(currentPosition);
                    }
                    else
                    {
                        (int x, int y) claySpotToLeft = default;
                        (int x, int y) openingToLeft = default;

                        (int x, int y) left = (currentPosition.x - 1, currentPosition.y);
                        var passedSpots = new List<(int x, int y)> { currentPosition };
                        while (true)
                        {
                            if (claySpots.Contains(left))
                            {
                                claySpotToLeft = left;
                                break;
                            }
                            var below = (left.x, left.y + 1);
                            passedSpots.Add(left);
                            if (!settledWaterSpots.Contains(below) && !claySpots.Contains(below))
                            {
                                openingToLeft = left;
                                break;
                            }
                            left = (left.x - 1, left.y);
                        }

                        (int x, int y) claySpotToRight = default;
                        (int x, int y) openingToRight = default;

                        (int x, int y) right = (currentPosition.x + 1, currentPosition.y);
                        while (true)
                        {
                            if (claySpots.Contains(right))
                            {
                                claySpotToRight = right;
                                break;
                            }
                            var below = (right.x, right.y + 1);
                            passedSpots.Add(right);
                            if (!settledWaterSpots.Contains(below) && !claySpots.Contains(below))
                            {
                                openingToRight = right;
                                break;
                            }
                            right = (right.x + 1, right.y);
                        }

                        if (claySpotToLeft != default && claySpotToRight != default)
                        {
                            foreach (var spot in passedSpots)
                            {
                                settledWaterSpots.Add(spot);
                            }
                            break;
                        }

                        foreach (var spot in passedSpots)
                        {
                            seenWaterSpots.Add(spot);
                        }

                        if (openingToLeft != default && deadEnds.Contains(openingToLeft)) openingToLeft = default;
                        if (openingToRight != default && deadEnds.Contains(openingToRight)) openingToRight = default;
                        if (openingToLeft != default)
                        {
                            latestDropPosition = openingToLeft;
                            currentPosition = openingToLeft;
                        }
                        else if (openingToRight != default)
                        {
                            latestDropPosition = openingToRight;
                            currentPosition = openingToRight;
                        }
                        else
                        {
                            deadEnds.Add(latestDropPosition);
                            break;
                        }
                    }
                }
            }
            if (shouldDraw) Draw(waterStart, claySpots, settledWaterSpots, seenWaterSpots);

            return (settledWaterSpots.Count, seenWaterSpots.Except(settledWaterSpots).Where(w => w.y >= minY).Count());
        }

        private static void Draw((int x, int y) waterStart, HashSet<(int x, int y)> claySpots, HashSet<(int x, int y)> settledWaterSpots, HashSet<(int x, int y)> seenWaterSpots)
        {
            var minClay = claySpots.Min(p => p.x);
            var minSeen = seenWaterSpots.Min(p => p.x);
            var minSettled = settledWaterSpots.Min(p => p.x);
            var minX = Math.Min(Math.Min(minClay, minSeen), minSettled);
            var maxClay = claySpots.Max(p => p.x);
            var maxSeen = seenWaterSpots.Max(p => p.x);
            var maxSettled = settledWaterSpots.Max(p => p.x);
            var maxX = Math.Max(Math.Max(maxClay, maxSeen), maxSettled);
            var minY = claySpots.Min(p => p.y);
            var maxY = claySpots.Max(p => p.y);
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var pos = (x, y);
                    char c;
                    if (waterStart == pos) c = '+';
                    else if (claySpots.Contains(pos)) c = '#';
                    else if (settledWaterSpots.Contains(pos)) c = '~';
                    else if (seenWaterSpots.Contains(pos)) c = '|';
                    else c = '.';
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static HashSet<(int x, int y)> GetClayCells(IList<string> lines)
        {
            var clayCells = new HashSet<(int x, int y)>();

            foreach (var vLine in lines.Where(l => l.StartsWith('x')).ToList())
            {
                var parts = vLine.Replace("x=", "").Split(", y=");
                var x = int.Parse(parts[0]);
                var yParts = parts[1].Split("..");
                var yMin = int.Parse(yParts[0]);
                var yMax = int.Parse(yParts[1]);

                for (var y = yMin; y <= yMax; y++)
                {
                    clayCells.Add((x, y));
                }
            }

            foreach (var vLine in lines.Where(l => l.StartsWith('y')).ToList())
            {
                var parts = vLine.Replace("y=", "").Split(", x=");
                var y = int.Parse(parts[0]);
                var xParts = parts[1].Split("..");
                var xMin = int.Parse(xParts[0]);
                var xMax = int.Parse(xParts[1]);

                for (var x = xMin; x <= xMax; x++)
                {
                    clayCells.Add((x, y));
                }
            }
            return clayCells;
        }
    }
}
