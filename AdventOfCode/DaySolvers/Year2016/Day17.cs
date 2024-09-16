using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = lines[0];
            var expectedResult = "DRLRDDURDR";
            var result = SolveEasy(input);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var input = lines[0];
            var expectedResult = 500;
            var result = SolveHard(input);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string SolveEasy(string input)
        {
            var startingLocation = (x:0, y:0);
            var targetLocation = (x:3, y:3);
            var directions = new List<(Direction direction, string letter)> { (Direction.North, "U"), (Direction.South, "D"), (Direction.West, "L"), (Direction.East, "R") };
            var queue = new PriorityQueue<((int x, int y) location, string path), int>();
            queue.Enqueue((startingLocation, ""), 0);

            while (true)
            {
                var (location, path) = queue.Dequeue();
                if (location == targetLocation) return path;

                var hash = MD5Helper.HashString(input + path).ToLower();

                var movements = new List<(Direction direction, string letter)>();
                for (var i = 0; i < 4; i++)
                {
                    if ("bcdef".Contains(hash[i])) movements.Add(directions[i]);
                }
                Console.WriteLine($"Current Location: ({location.x},{location.y}); Current Path: {path}; Current Hash: {hash[..4]}; Movements:{string.Join(",", movements.Select(m => m.letter))}");
                foreach (var (direction, letter) in movements)
                {
                    var (x, y) = DirectionExtensions.GetMovement(direction, location);
                    if (x < startingLocation.x || x > targetLocation.x || y < startingLocation.y || y > targetLocation.y) continue;
                    var newPath = path + letter;
                    queue.Enqueue(((x, y), newPath), newPath.Length);
                }
            }
        }

        private static int SolveHard(string input)
        {
            var startingLocation = (x: 0, y: 0);
            var targetLocation = (x: 3, y: 3);

            var directions = new List<(Direction direction, string letter)> { (Direction.North, "U"), (Direction.South, "D"), (Direction.West, "L"), (Direction.East, "R") };
            var queue = new PriorityQueue<((int x, int y) location, string path), int>();

            queue.Enqueue((startingLocation, ""), 0);
            var longestPathLength = 0;
            while (true)
            {
                if (queue.Count == 0) break;
                var (location, path) = queue.Dequeue();
                
                if (location == targetLocation)
                {
                    if (path.Length > longestPathLength) longestPathLength = path.Length;
                    continue;
                }

                var hash = MD5Helper.HashString(input + path).ToLower();

                var movements = new List<(Direction direction, string letter)>();
                for (var i = 0; i < 4; i++)
                {
                    if ("bcdef".Contains(hash[i])) movements.Add(directions[i]);
                }

                foreach (var (direction, letter) in movements)
                {
                    var (x, y) = DirectionExtensions.GetMovement(direction, location);
                    if (x < startingLocation.x || x > targetLocation.x || y < startingLocation.y || y > targetLocation.y) continue;
                    var newPath = path + letter;
                    queue.Enqueue(((x, y), newPath), newPath.Length);
                }
            }
            return longestPathLength;
        }
    }
}
