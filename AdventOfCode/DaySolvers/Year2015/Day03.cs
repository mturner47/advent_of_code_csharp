using Helpers.Helpers;

namespace AdventOfCode.Year2015
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var visitedHouses = new List<(int x, int y)>();
            var currentLocation = (x:0, y:0);
            visitedHouses.Add(currentLocation);
            var directions = new Dictionary<char, Direction>
            {
                ['^'] = Direction.North,
                ['<'] = Direction.West,
                ['>'] = Direction.East,
                ['v'] = Direction.South,
            };
            foreach (var c in lines[0])
            {
                var direction = directions[c];
                currentLocation = direction.GetMovement(currentLocation);
                visitedHouses.Add(currentLocation);
            }
            var expectedResult = 2592;
            var result = visitedHouses.Distinct().Count();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var visitedHouses = new List<(int x, int y)>();
            var santaLocation = (x: 0, y: 0);
            var robotLocation = (x: 0, y: 0);
            visitedHouses.Add(santaLocation);
            var directions = new Dictionary<char, Direction>
            {
                ['^'] = Direction.North,
                ['<'] = Direction.West,
                ['>'] = Direction.East,
                ['v'] = Direction.South,
            };
            var line = lines[0];
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                var direction = directions[c];
                if (i % 2 == 0)
                {
                    santaLocation = direction.GetMovement(santaLocation);
                    visitedHouses.Add(santaLocation);
                }
                else
                {
                    robotLocation = direction.GetMovement(robotLocation);
                    visitedHouses.Add(robotLocation);
                }
            }
            var expectedResult = 2360;
            var result = visitedHouses.Distinct().Count();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
