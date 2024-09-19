using Helpers.Helpers;

namespace AdventOfCode.Year2018
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var line = lines[0];
            line = line[1..^1];
            var connections = GetConnections(line);
            var knownDistances = GetDistances(connections);

            var expectedResult = 4025;
            var result = knownDistances.Values.Max();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var line = lines[0];
            line = line[1..^1];
            var connections = GetConnections(line);
            var knownDistances = GetDistances(connections);

            var expectedResult = 8186;
            var result = knownDistances.Values.Count(d => d >= 1000);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static HashSet<((int x, int y) a, (int x, int y) b)> GetConnections(string line)
        {
            var currentPositions = new HashSet<(int x, int y)> { (0, 0) };
            var connections = new HashSet<((int x, int y) a, (int x, int y) b)>();
            var starts = new HashSet<(int x, int y)> { { (0, 0) } };
            var ends = new HashSet<(int x, int y)>();
            var stack = new Stack<(HashSet<(int x, int y)> starts, HashSet<(int x, int y)> ends)>();
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if ("NEWS".Contains(c))
                {
                    var direction = DirectionExtensions.ParseChar(c);
                    foreach (var p in currentPositions) connections.Add((p, direction.GetMovement(p)));
                    currentPositions = currentPositions.Select(p => direction.GetMovement(p)).ToHashSet();
                }
                else if (c == '|')
                {
                    foreach (var p in currentPositions) ends.Add(p);
                    currentPositions = starts;
                }
                else if (c == '(')
                {
                    stack.Push((starts, ends));
                    starts = currentPositions;
                    ends = [];
                }
                else if (c == ')')
                {
                    foreach (var e in ends) currentPositions.Add(e);
                    (starts, ends) = stack.Pop();
                }
            }
            return connections;
        }

        private static Dictionary<(int x, int y), int> GetDistances(HashSet<((int x, int y) a, (int x, int y) b)> connections)
        {
            var positionsToCheck = new PriorityQueue<(int x, int y), int>();
            positionsToCheck.Enqueue((0, 0), 0);
            var knownDistances = new Dictionary<(int x, int y), int> { { (0, 0), 0 } };

            while (positionsToCheck.Count > 0)
            {
                var pos = positionsToCheck.Dequeue();
                foreach (var (a, b) in connections.Where(c => c.a == pos || c.b == pos))
                {
                    var other = a == pos ? b : a;
                    if (knownDistances.ContainsKey(other)) continue;
                    var distance = knownDistances[pos] + 1;
                    knownDistances[other] = distance;
                    positionsToCheck.Enqueue(other, distance);
                }
            }
            return knownDistances;
        }
    }
}
