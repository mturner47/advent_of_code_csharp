using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day13 : IDaySolver
    {
        private static readonly Dictionary<(int, int), (bool isWall, int bestSteps)> _knownLocations = new() { { (1, 1), (false, 0) } };

        public object EasySolution(IList<string> lines)
        {
            var favoriteNumber = int.Parse(lines[0]);
            var targetLocation = (31, 39);

            var expectedResult = 82;
            var result = Solve(favoriteNumber, targetLocation);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var favoriteNumber = int.Parse(lines[0]);
            var reachableLocationCount = 0;
            for (var x = 0; x < 51; x++)
            {
                for (var y = 0; y < 51; y++)
                {
                    var distance = Solve(favoriteNumber, (x, y));
                    if (distance <= 50) reachableLocationCount++;
                }
            }
            var expectedResult = 138;
            var result = reachableLocationCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(int favoriteNumber, (int, int) targetLocation)
        {
            var startingLocation = (1, 1);

            var queue = new PriorityQueue<((int, int) currentLocation, int numSteps), long>();
            queue.Enqueue((startingLocation, 0), 0L + MathHelpers.ManhattanDistance(startingLocation, targetLocation));

            while (true)
            {
                if (queue.Count == 0) return 51;
                var (currentLocation, currentSteps) = queue.Dequeue();
                if (currentLocation == targetLocation)
                {
                    return currentSteps;
                }

                foreach (var (x, y) in DirectionExtensions.GetAllMovements(currentLocation))
                {
                    if (x < 0) continue;
                    if (y < 0) continue;

                    bool isWall;
                    if (!_knownLocations.ContainsKey((x, y)))
                    {
                        isWall = CalculateIsWall(x, y, favoriteNumber);
                        _knownLocations.Add((x, y), (isWall, currentSteps + 1));
                    }
                    else
                    {
                        (isWall, var bestSteps) = _knownLocations[(x, y)];
                        if (bestSteps < currentSteps) continue;
                    }

                    if (isWall) continue;

                    queue.Enqueue(((x, y), currentSteps + 1), currentSteps + 1 + MathHelpers.ManhattanDistance((x, y), targetLocation));
                }
            }
        }

        private static bool CalculateIsWall(int x, int y, int favoriteNumber)
        {
            var value = x * x + 3 * x + 2 * x * y + y + y * y;
            value += favoriteNumber;
            var bitValue = Convert.ToString(value, 2);
            return bitValue.Count(c => c == '1') % 2 == 1;
        }
    }
}
