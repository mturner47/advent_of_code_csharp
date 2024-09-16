using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day24 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 428;
            var result = Solve(lines, false);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var result =  Solve(lines, true);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(IList<string> lines, bool mustReturnToStart)
        {
            string nodes = "01234567";
            var nodeLocations = FindNodeLocations(lines, nodes);

            var nodeCombos = MathHelpers.GetCombinations(nodes.ToList()).Where(c => c.Count == 2).ToDictionary(c => (c[0], c[1]), c => int.MaxValue);

            foreach (var nodeCombo in nodeCombos)
            {
                var node1 = nodeCombo.Key.Item1;
                var node2 = nodeCombo.Key.Item2;
                nodeCombos[nodeCombo.Key] = FindShortestPath(nodeLocations[node1], nodeLocations[node2], lines);
            }

            return FindShortestPathToVisitAllNodes(nodeCombos, mustReturnToStart);
        }

        private static int FindShortestPath((int x, int y) start, (int x, int y) end, IList<string> lines)
        {
            var knownLocations = new List<(int x, int y)>() { start };
            var nodesToExpand = new PriorityQueue<((int x, int y), int distance), int>();
            nodesToExpand.Enqueue((start, 0), 0);

            while (true)
            {

                var (node, distance) = nodesToExpand.Dequeue();
                var newDistance = distance + 1;
                foreach (var (x, y) in DirectionExtensions.GetAllMovements(node))
                {
                    if ((x, y) == end) return newDistance;
                    var c = lines[y][x];
                    if (c == '.' && !knownLocations.Contains((x, y)))
                    {
                        knownLocations.Add((x, y));
                        nodesToExpand.Enqueue(((x, y), newDistance), newDistance);
                    }
                }
            }
        }

        private static Dictionary<char, (int x, int y)> FindNodeLocations(IList<string> lines, string nodes)
        {
            var dict = nodes.ToDictionary(n => n, n => (0, 0));
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if (c != '#' && c != '.')
                    {
                        dict[c] = (x, y);
                    }
                }
            }
            return dict;
        }

        private static int FindShortestPathToVisitAllNodes(Dictionary<(char, char), int> nodeCombos, bool mustReturnToStart)
        {
            var queue = new PriorityQueue<(char currentNode, int distance, string path), int>();

            queue.Enqueue(('0', 0, "0"), 0);

            while (true)
            {
                var (currentNode, distance, path) = queue.Dequeue();
                if (path.Length == (mustReturnToStart ? 9 : 8)) return distance;

                foreach (var kvp in nodeCombos.Where(kvp => kvp.Key.Item1 == currentNode || kvp.Key.Item2 == currentNode))
                {
                    var newNode = kvp.Key.Item1 == currentNode ? kvp.Key.Item2 : kvp.Key.Item1;
                    if (path.Contains(newNode))
                    {
                        if (!mustReturnToStart) continue;
                        if (newNode != '0') continue;
                        if (path.Length != 8) continue;
                    }
                    var newDistance = distance + kvp.Value;
                    var newPath = path + newNode;
                    queue.Enqueue((newNode, newDistance, newPath), newDistance);
                }
            }
        }
    }
}
