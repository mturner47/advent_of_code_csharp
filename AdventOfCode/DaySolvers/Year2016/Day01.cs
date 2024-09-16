using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var movements = ParseLine(lines[0]);
            var currentPosition = (0L, 0L);
            var currentDirection = Direction.North;
            foreach (var (direction, count) in movements)
            {
                currentDirection = direction == 'L' ? currentDirection.GetCCW() : currentDirection.GetCW();
                currentPosition = currentDirection.GetMovement(currentPosition, count);
            }
            var expectedResult = 300;
            var result = MathHelpers.ManhattanDistance((0L, 0L), currentPosition);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var movements = ParseLine(lines[0]);
            var currentPosition = (x: 0L, y: 0L);
            var currentDirection = Direction.North;
            var visitedLocations = new List<(long, long)> { currentPosition };
            var found = false;
            foreach (var (direction, count) in movements)
            {
                currentDirection = direction == 'L' ? currentDirection.GetCCW() : currentDirection.GetCW();
                for (var i = 0; i < count; i++)
                {
                    currentPosition = currentDirection.GetMovement(currentPosition, 1);
                    if (visitedLocations.Contains(currentPosition))
                    {
                        found = true;
                        break;
                    }
                    else
                    {
                        visitedLocations.Add(currentPosition);
                    }
                }
                if (found) break;
            }

            var expectedResult = 159;
            var result = MathHelpers.ManhattanDistance((0L, 0L), currentPosition);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<(char direction, int count)> ParseLine(string line)
        {
            return line.Split(", ").Select(m => (m[0], int.Parse(m[1..]))).ToList();
        }

        private static void PrintGrid(List<(long x, long y)> grid)
        {
            var minY = grid.Min(c => c.y);
            var maxY = grid.Max(c => c.y);
            var minX = grid.Min(c => c.x);
            var maxX = grid.Max(c => c.x);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(grid.Contains((x, y)) ? '@': '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
