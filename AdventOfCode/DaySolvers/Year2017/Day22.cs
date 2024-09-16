using Helpers.Helpers;

namespace AdventOfCode.Year2017
{
    internal class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var startingLocation = (lines.Count / 2, lines[0].Length / 2);
            var infectedPoints = new List<(int x, int y)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') infectedPoints.Add((x, y));
                }
            }

            var currentPosition = startingLocation;
            var currentDirection = Direction.North;
            var infectionCount = 0;
            for (var i = 0; i < 10000; i++)
            {
                if (infectedPoints.Contains(currentPosition))
                {
                    currentDirection = currentDirection.GetCW();
                    infectedPoints.Remove(currentPosition);
                }
                else
                {
                    currentDirection = currentDirection.GetCCW();
                    infectedPoints.Add(currentPosition);
                    infectionCount++;
                }
                currentPosition = currentDirection.GetMovement(currentPosition);
            }
            var expectedResult = 5280;
            var result = infectionCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var startingLocation = (lines.Count / 2, lines[0].Length / 2);
            var infectedPoints = new List<(int x, int y)>();
            var weakenedPoints = new List<(int x, int y)>();
            var flaggedPoints = new List<(int x, int y)>();

            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') infectedPoints.Add((x, y));
                }
            }

            var currentPosition = startingLocation;
            var currentDirection = Direction.North;
            var infectionCount = 0;
            for (var i = 0; i < 10000000; i++)
            {
                if (infectedPoints.Contains(currentPosition))
                {
                    currentDirection = currentDirection.GetCW();
                    infectedPoints.Remove(currentPosition);
                    flaggedPoints.Add(currentPosition);
                }
                else if (flaggedPoints.Contains(currentPosition))
                {
                    currentDirection = currentDirection.GetCW().GetCW();
                    flaggedPoints.Remove(currentPosition);
                }
                else if (weakenedPoints.Contains(currentPosition))
                {
                    infectedPoints.Add(currentPosition);
                    weakenedPoints.Remove(currentPosition);
                    infectionCount++;
                }
                else
                {
                    currentDirection = currentDirection.GetCCW();
                    weakenedPoints.Add(currentPosition);
                }
                currentPosition = currentDirection.GetMovement(currentPosition);
            }

            var expectedResult = 2512261;
            var result = infectionCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
