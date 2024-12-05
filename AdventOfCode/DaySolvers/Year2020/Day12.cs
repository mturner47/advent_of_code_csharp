using Helpers.Helpers;

namespace AdventOfCode.Year2020
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var currentPosition = (x: 0, y: 0);
            var currentDirection = Direction.East;
            foreach (var l in lines)
            {
                var direction = l[0];
                var distance = int.Parse(l[1..]);
                if (direction == 'F')
                {
                    currentPosition = currentDirection.GetMovement(currentPosition, distance);
                }
                else if ("NEWS".Contains(direction))
                {
                    currentPosition = DirectionExtensions.ParseChar(direction).GetMovement(currentPosition, distance);
                }
                else if (direction == 'L' && distance == 90 || direction == 'R' && distance == 270)
                {
                    currentDirection = currentDirection.GetCCW();
                }
                else if (direction == 'R' && distance == 90 || direction == 'L' && distance == 270)
                {
                    currentDirection = currentDirection.GetCW();
                }
                else currentDirection = currentDirection.Reverse();
            }

            var expectedResult = 923;
            var result = MathHelpers.ManhattanDistance((0, 0), currentPosition);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var currentPosition = (x: 0, y: 0);
            var waypointOffset = (x: 10, y: -1);
            foreach (var l in lines)
            {
                var direction = l[0];
                var distance = int.Parse(l[1..]);
                if (direction == 'F')
                {
                    var xDistance = waypointOffset.x * distance;
                    var yDistance = waypointOffset.y * distance;
                    currentPosition = (currentPosition.x + xDistance, currentPosition.y + yDistance);
                }
                else if ("NEWS".Contains(direction))
                {
                    waypointOffset = DirectionExtensions.ParseChar(direction).GetMovement(waypointOffset, distance);
                }
                else if (direction == 'L' && distance == 90 || direction == 'R' && distance == 270)
                {
                    waypointOffset = (waypointOffset.y, -waypointOffset.x); 
                }
                else if (direction == 'R' && distance == 90 || direction == 'L' && distance == 270)
                {
                    waypointOffset = (-waypointOffset.y, waypointOffset.x);
                }
                else waypointOffset = (-waypointOffset.x, -waypointOffset.y);
            }


            var expectedResult = 24769;
            var result = MathHelpers.ManhattanDistance((0, 0), currentPosition);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
