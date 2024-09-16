using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(ParseChar).ToList()).ToList();
            Dictionary<char, (int x, int y, Cell cell)> keys = [];
            Dictionary<char, (int x, int y, Cell cell)> entrances = [];

            for (var y = 0; y < grid.Count; y++)
            {
                var row = grid[y];
                for (var x = 0; x < row.Count; x++)
                {
                    var cell = row[x];
                    cell.Position = (x, y);
                    if (cell.Type == CellType.Key) keys.Add(cell.Name, (x, y, cell));
                    if (cell.Type == CellType.Entrance) entrances.Add(cell.Name, (x, y, cell));
                }
            }

            var nodes = keys.Values.Concat(entrances.Values).ToList();

            var edges = nodes.ToDictionary(node => node.cell.Name, node => GetReachableNodes(grid, node.cell));
            var expectedResult = 5402;
            var result = CountSteps(edges, entrances);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            FixLines(lines);
            var grid = lines.Select(l => l.Select(ParseChar).ToList()).ToList();
            Dictionary<char, (int x, int y, Cell cell)> keys = [];
            Dictionary<char, (int x, int y, Cell cell)> entrances = [];

            for (var y = 0; y < grid.Count; y++)
            {
                var row = grid[y];
                for (var x = 0; x < row.Count; x++)
                {
                    var cell = row[x];
                    cell.Position = (x, y);
                    if (cell.Type == CellType.Key) keys.Add(cell.Name, (x, y, cell));
                    if (cell.Type == CellType.Entrance) entrances.Add(cell.Name, (x, y, cell));
                }
            }

            var nodes = keys.Values.Concat(entrances.Values).ToList();

            var edges = nodes.ToDictionary(node => node.cell.Name, node => GetReachableNodes(grid, node.cell));
            var expectedResult = 2138;
            var result = CountSteps(edges, entrances);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<Edge> GetReachableNodes(List<List<Cell>> grid, Cell cell)
        {
            var edges = new List<Edge>();
            var foundKeys = "";
            var queue = new PriorityQueue<((int x, int y) currentPosition, List<(int x, int y)> path, ulong keysRequired), int>();
            queue.Enqueue((cell.Position, [], 0), 0);
            while (queue.Count > 0)
            {
                var (position, path, keysRequired) = queue.Dequeue();
                foreach (var (adjX, adjY) in DirectionExtensions.GetAllMovements(position))
                {
                    if (path.Contains((adjX, adjY))) continue;

                    var adjCell = grid[adjY][adjX];
                    if (adjCell.Name == cell.Name) continue;

                    var type = adjCell.Type;
                    if (type == CellType.Wall) continue;
                    if (type == CellType.Key)
                    {
                        if (!foundKeys.Contains(adjCell.Name))
                        {
                            foundKeys += adjCell.Name;
                            edges.Add(new Edge { ConnectedKey = adjCell, Distance = path.Count + 1, KeysRequired = keysRequired });
                        }
                    }

                    var newKeysRequired = keysRequired;
                    if (type == CellType.Door)
                    {
                        newKeysRequired = keysRequired | adjCell.Flag;
                    }

                    var newPath = path.ToList();
                    newPath.Add(position);
                    queue.Enqueue(((adjX, adjY), newPath, newKeysRequired), newPath.Count);
                }
            }

            return edges;
        }

        private static long CountSteps(Dictionary<char, List<Edge>> edges, Dictionary<char, (int x, int y, Cell cell)> entrances)
        {
            var startingStates = new string(entrances.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Key).ToArray());

            var bestStates = new Dictionary<(ulong, string), (long distance, string path)>();
            ulong goalState = (ulong)Math.Pow(2, edges.Keys.Count - entrances.Keys.Count) - 1;
            var bestGoal = (distance:long.MaxValue, path:"");

            var queue = new PriorityQueue<(ulong collectedKeys, string nextNodes, long totalDistance, string path), long>();
            foreach (var entranceKvp in entrances)
            {
                var robotName = entranceKvp.Key;
                var robotEdges = edges[robotName];
                foreach (var edge in robotEdges.Where(e => e.KeysRequired == 0))
                {
                    var connectedNode = edge.ConnectedKey;
                    var path = "" + connectedNode.Name;
                    var bestStatesPositions = startingStates.Replace(robotName, connectedNode.Name);
                    bestStates[(connectedNode.Flag, bestStatesPositions)] = (edge.Distance, path);
                    queue.Enqueue((connectedNode.Flag, bestStatesPositions, edge.Distance, path), edge.Distance);
                }
            }

            while (queue.Count > 0)
            {
                var (collectedKeys, nextNodes, totalDistance, path) = queue.Dequeue();

                foreach (var (nodeName, edge) in nextNodes.SelectMany(c => edges[c].Select(e => (c, e))))
                {
                    if ((edge.KeysRequired | collectedKeys) != collectedKeys) continue;

                    var connectedNode = edge.ConnectedKey;

                    var newCollectedKeys = collectedKeys | edge.ConnectedKey.Flag;
                    if (newCollectedKeys == collectedKeys) continue;

                    var newDistance = totalDistance + edge.Distance;
                    var newPositions = nextNodes.Replace(nodeName, connectedNode.Name);
                    if (bestStates.TryGetValue((newCollectedKeys, newPositions), out (long bestDistance, string path) value) && value.bestDistance <= newDistance) continue;

                    var newPath = path + edge.ConnectedKey.Name;
                    bestStates[(newCollectedKeys, newPositions)] = (totalDistance + edge.Distance, newPath);

                    if (newCollectedKeys == goalState)
                    {
                        if (bestGoal.distance > newDistance)
                        {
                            bestGoal = (newDistance, newPath);
                        }
                        continue;
                    }
                    queue.Enqueue((newCollectedKeys, newPositions, newDistance, newPath), newDistance);
                }
            }

            return bestGoal.distance;
        }

        private static void FixLines(IList<string> lines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var entranceIndex = line.IndexOf('@');
                if (entranceIndex > -1)
                {
                    lines[i] = line.Replace(".@.", "###");
                    lines[i - 1] = lines[i - 1][..(entranceIndex - 1)] + "1#2" + lines[i - 1][(entranceIndex + 2)..];
                    lines[i + 1] = lines[i + 1][..(entranceIndex - 1)] + "3#4" + lines[i + 1][(entranceIndex + 2)..];
                    break;
                }
            }
        }

        private class Edge
        {
            public required Cell ConnectedKey { get; set; }
            public int Distance { get; set; }
            public ulong KeysRequired { get; set; } = 0;
        }

        private class Cell
        {
            public char Name { get; set; }
            public CellType Type { get; set; }
            public bool IsUsed { get; set; }
            public (int x, int y) Position { get; set; }
            public ulong Flag { get; set; }
        }

        private static Cell ParseChar(char c)
        {
            var type = c switch
            {
                '@' or '1' or '2' or '3' or '4' => CellType.Entrance,
                '.' => CellType.Open,
                '#' => CellType.Wall,
                >= 'a' => CellType.Key,
                <= 'Z' => CellType.Door,
                _ => throw new NotImplementedException(),
            };

            ulong flag = 0;
            ulong baseFlag = 0b1;
            if (type == CellType.Key || type == CellType.Door)
            {
                var letter = type == CellType.Key ? c : char.ToLower(c);
                flag = baseFlag << (letter - 'a');
            }

            return new Cell
            {
                Type = type,
                Name = c,
                IsUsed = false,
                Flag = flag,
            };
        }

        private enum CellType
        {
            Entrance,
            Open,
            Door,
            Wall,
            Key,
        }
    }
}
