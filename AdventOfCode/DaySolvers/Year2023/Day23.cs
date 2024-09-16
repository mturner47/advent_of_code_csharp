using Helpers.Helpers;

namespace AdventOfCode.Year2023
{
    internal class Day23 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = ParseInput(lines);
            var maxX = grid[0].Count - 1;
            var maxY = grid.Count - 1;
            var startingPoint = (grid[0].FindIndex(c => c.Type == CellType.Path), 0);
            var endingPoint = (grid[maxY].FindIndex(c => c.Type == CellType.Path), maxY);

            var paths = new PriorityQueue<List<(int x, int y)>, int>();
            paths.Enqueue([startingPoint], 0);

            var directions = DirectionExtensions.EnumerateDirections();

            var maxLength = 0;
            var bestPath = new List<(int x, int y)>();
            while (paths.Count > 0)
            {
                var path = paths.Dequeue();
                var (x, y) = path.Last();
                foreach (var direction in directions)
                {
                    var (nextX, nextY) = direction.GetMovement((x, y));
                    if (path.Contains((nextX, nextY))) continue;
                    if (nextX < 0 || nextX > maxX || nextY < 0 || nextY > maxY) continue;

                    var nextCell = grid[nextY][nextX];
                    if (nextCell.Type == CellType.Forest) continue;
                    if (nextCell.Type == CellType.Slope && nextCell.Dir != direction) continue;

                    var newPath = path.Select(p => p).ToList();
                    newPath.Add((nextX, nextY));

                    if ((nextX, nextY) == endingPoint)
                    {
                        if (newPath.Count - 1 > maxLength)
                        {
                            maxLength = newPath.Count - 1;
                            bestPath = newPath;
                        }
                        continue;
                    }

                    paths.Enqueue(newPath, 1 - newPath.Count);
                }
            }

            return maxLength;
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = ParseInput(lines);

            var nodes = ConvertGridToNodes(grid, false);
            return GetMaxLength(nodes);
        }

        private static int GetMaxLength(List<Node> nodes)
        {
            ulong currentFlag = 0b1;
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                node.Flag = currentFlag;
                currentFlag <<= 1;
            }

            var startingNode = nodes.First(n => n.IsStartingNode);

            var pathsToCheck = new Stack<(ulong path, Node node, int totalLength)>();
            pathsToCheck.Push((startingNode.Flag, startingNode, 0));

            var maxLength = 0;
            while (pathsToCheck.Count > 0)
            {
                var (path, node, totalLength) = pathsToCheck.Pop();
                foreach (var (connectedNode, weight, _) in node.ConnectedEdges)
                {
                    var newPath = connectedNode.Flag | path;
                    if (newPath == path) continue;
                    var newLength = totalLength + weight;
                    if (!connectedNode.IsEndingNode)
                    {
                        pathsToCheck.Push((newPath, connectedNode, newLength));
                    }
                    else
                    {
                        if (maxLength < newLength) maxLength = newLength;
                    }
                }
            }
            return maxLength;
        }

        private static List<Node> ConvertGridToNodes(List<List<Cell>> grid, bool shouldSlopesRestrict)
        {
            var maxX = grid[0].Count - 1;
            var maxY = grid.Count - 1;
            var (startX, startY) = (grid[0].FindIndex(c => c.Type == CellType.Path), 0);

            var startingCell = grid[startY][startX];
            var startingNode = new Node { Cell = startingCell, IsStartingNode = true, };
            var nodeDict = new Dictionary<(int x, int y), Node> { { (startX, startY), startingNode } };

            var nodesToCheck = new Stack<(int x, int y, Node n)>();
            nodesToCheck.Push((startX, startY, startingNode));

            var directions = DirectionExtensions.EnumerateDirections();

            while(nodesToCheck.Count > 0)
            {
                var (x, y, node) = nodesToCheck.Pop();

                foreach (var direction in directions)
                {
                    if (node.ConnectedEdges.Any(ce => ce.dir == direction)) continue;
                    var dir = direction;
                    var (nextX, nextY) = dir.GetMovement((x, y));
                    if (nextX < 0 || nextX > maxX || nextY < 0 || nextY > maxY) continue;

                    var nextCell = grid[nextY][nextX];
                    if (nextCell.Type == CellType.Forest) continue;
                    if (shouldSlopesRestrict && nextCell.Type == CellType.Slope && nextCell.Dir != direction) continue;

                    var pathLength = 1;

                    while (true)
                    {
                        var connectedCells = directions
                            .Where(d => d != dir.GetOpposite())
                            .Select(d => (direction: d, point: d.GetMovement((nextX, nextY))))
                            .Where(i => i.point.x >= 0 && i.point.x <= maxX && i.point.y >= 0 && i.point.y <= maxY)
                            .Select(i => (i.direction, i.point, cell: grid[i.point.y][i.point.x]))
                            .Where(i =>
                            {
                                if (i.cell.Type == CellType.Path) return true;
                                if (i.cell.Type == CellType.Forest) return false;
                                if (!shouldSlopesRestrict) return true;
                                return i.cell.Dir == i.direction;
                            })
                            .ToList();

                        if (connectedCells.Count == 1)
                        {
                            pathLength++;
                            (dir, (nextX, nextY), _) = connectedCells[0];
                        }
                        else
                        {
                            var cell = grid[nextY][nextX];
                            if (!nodeDict.ContainsKey((nextX, nextY)))
                            {
                                nodeDict.Add((nextX, nextY), new Node { Cell = cell });
                                nodesToCheck.Push((nextX, nextY, nodeDict[(nextX, nextY)]));
                            }
                            var newNode = nodeDict[(nextX, nextY)];
                            newNode.ConnectedEdges.Add((node, pathLength, dir.GetOpposite()));
                            node.ConnectedEdges.Add((newNode, pathLength, direction));
                            break;
                        }
                    }
                }
            }

            var endingPoint = (grid[maxY].FindIndex(c => c.Type == CellType.Path), maxY);
            nodeDict[endingPoint].IsEndingNode = true;
            return nodeDict.Select(kvp => kvp.Value).ToList();
        }

        private static List<List<Cell>> ParseInput(IList<string> lines)
        {
            return lines.Select(l => l.Select(c =>
            {
                var cell = c switch
                {
                    '#' => new Cell { Type = CellType.Forest, Dir = null },
                    '.' => new Cell { Type = CellType.Path, Dir = null },
                    '^' => new Cell { Type = CellType.Slope, Dir = Direction.North },
                    '>' => new Cell { Type = CellType.Slope, Dir = Direction.East },
                    '<' => new Cell { Type = CellType.Slope, Dir = Direction.West },
                    'v' => new Cell { Type = CellType.Slope, Dir = Direction.South },
                    _ => throw new NotImplementedException(),
                };
                return cell;
            }).ToList()).ToList();
        }

        private class Node
        {
            public Cell Cell { get; set; } = new();
            public List<(Node connectedNode, int weight, Direction dir)> ConnectedEdges { get; set; } = [];
            public bool IsStartingNode { get; set; } = false;
            public bool IsEndingNode { get; set; } = false;
            public ulong Flag { get; set; }
        }

        private class Cell
        {
            public CellType Type { get; set; }
            public Direction? Dir { get; set; }
        }

        private enum CellType
        {
            Path,
            Forest,
            Slope,
        }
    }
}
