using Helpers.Helpers;

namespace AdventOfCode.Year2018
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var coordinates = lines.Select(Parse).ToList();
            var minX = coordinates.Min(p => p.x);
            var minY = coordinates.Min(p => p.y);
            var maxX = coordinates.Max(p => p.x);
            var maxY = coordinates.Max(p => p.y);

            var possibleOptions = coordinates.ToDictionary(c => c, _ => 0);
            var edgeCoordinates = new HashSet<(int x, int y)>();
            var extraRange = 100;

            for (var y = minY - extraRange; y < maxY + extraRange; y++)
            {
                for (var x = minX - extraRange; x < maxX + extraRange; x++)
                {
                    var distances = coordinates.Select(c => (coord: c, distance: MathHelpers.ManhattanDistance((x, y), c))).ToList();
                    var (coord, distance) = distances.OrderBy(d => d.distance).First();
                    if (distances.Where(d => d.distance == distance).Count() == 1)
                    {
                        possibleOptions[coord]++;
                        if (x == minX || x == maxX + extraRange - 1 || y == minY || y == maxY + extraRange - 1)
                        {
                            edgeCoordinates.Add(coord);
                        }
                    }
                }
            }

            var expectedResult = 3969;
            var result = possibleOptions.Where(p => !edgeCoordinates.Contains(p.Key)).Select(p => p.Value).Max();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var coordinates = lines.Select(Parse).ToList();
            var minX = coordinates.Min(p => p.x);
            var minY = coordinates.Min(p => p.y);
            var maxX = coordinates.Max(p => p.x);
            var maxY = coordinates.Max(p => p.y);

            var foundCoords = new List<(int x, int y)>();
            var maxDistanceAllowed = 10000;
            for (var y = minY; y < maxY; y++)
            {
                for (var x = minX; x < maxX; x++)
                {
                    var totalDistances = coordinates.Select(c => MathHelpers.ManhattanDistance((x, y), c)).ToList();
                    if (totalDistances.Sum() < maxDistanceAllowed) foundCoords.Add((x, y));
                }
            }

            var expectedResult = 42123;
            var result = foundCoords.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int x, int y) Parse(string line)
        {
            var parts = line.Split(", ");
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
