using Helpers.Helpers;
using System.Linq;

namespace AdventOfCode.Year2019
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = MakeGrid(lines);
            var expectedResult = 668;
            var result = CountSteps(grid);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = MakeGrid(lines);
            var expectedResult = 7778;
            var result = CountStepsRecursively(grid);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int CountSteps(Grid grid)
        {
            var queue = new PriorityQueue<((int x, int y) position, List<(int x, int y)> path, int distance), int>();
            queue.Enqueue((grid.Entrance, [], 0), 0);
            while (queue.Count > 0)
            {
                var (position, path, distance) = queue.Dequeue();
                var cell = grid.Get(position);
                if (cell.Type == CellType.Portal && !path.Contains(cell.MatchingPortalLocation))
                {
                    var newPath = path.ToList();
                    newPath.Add(position);
                    var newDistance = distance + 1;
                    queue.Enqueue((cell.MatchingPortalLocation, newPath, newDistance), newDistance);
                    continue;
                }

                foreach (var adjPosition in DirectionExtensions.GetAllMovements(position))
                {
                    if (path.Contains(adjPosition)) continue;

                    var adjacentCell = grid.Get(adjPosition);
                    if (adjacentCell.Type == CellType.Exit) return distance + 1;
                    if (adjacentCell.Type == CellType.Wall) continue;
                    var newPath = path.ToList();
                    newPath.Add(adjPosition);
                    var newDistance = distance + 1;

                    queue.Enqueue((adjPosition, newPath, newDistance), newDistance);
                }
            }
            throw new Exception("Exit not found");
        }

        private static int CountStepsRecursively(Grid grid)
        {
            var queue = new PriorityQueue<((int x, int y) position, int layer, int distance), int>();
            queue.Enqueue((grid.Entrance, 0, 0), 0);
            var seenLocations = new List<((int x, int y) position, int layer)> { (grid.Entrance, 0) };

            while (queue.Count > 0)
            {
                var (position, layer, distance) = queue.Dequeue();
                var cell = grid.Get(position);
                if (cell.Type == CellType.Portal && !(cell.IsOuterPortal && layer == 0))
                {
                    var newLayer = cell.IsOuterPortal ? layer - 1 : layer + 1;
                    var newPosition = cell.MatchingPortalLocation;
                    if (!seenLocations.Contains((newPosition, newLayer)))
                    {
                        seenLocations.Add((newPosition, newLayer));
                        var newDistance = distance + 1;
                        queue.Enqueue((newPosition, newLayer, newDistance), newLayer * 100 + newDistance);
                        continue;
                    }
                }

                foreach (var adjPosition in DirectionExtensions.GetAllMovements(position))
                {
                    if (seenLocations.Contains((adjPosition, layer))) continue;

                    var adjacentCell = grid.Get(adjPosition);
                    if (adjacentCell.Type == CellType.Exit && layer == 0) return distance + 1;
                    if (adjacentCell.Type == CellType.Wall) continue;
                    seenLocations.Add((adjPosition, layer));
                    var newDistance = distance + 1;

                    queue.Enqueue((adjPosition, layer, newDistance), layer * 100 + newDistance);
                }
            }
            throw new Exception("Exit not found");
        }

        private static Grid MakeGrid(IList<string> lines)
        {
            var portals = new Dictionary<string, ((int x, int y) portal1, (int x, int y) portal2)>();
            var grid = new List<List<Cell>>();
            var entrance = (x: 0, y: 0);
            var directions = DirectionExtensions.EnumerateDirections();

            for (var y = 0; y < lines.Count; y++)
            {
                var row = new List<Cell>();
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    var type = CellType.Wall;
                    var portalName = "";
                    var isOuterPortal = false;
                    if (c == '.')
                    {
                        type = CellType.Open;
                        foreach (var direction in directions)
                        {
                            var (adjx, adjy) = direction.GetMovement((x, y));
                            var adjacentCell = lines[adjy][adjx];
                            if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(adjacentCell))
                            {
                                var (nextx, nexty) = direction.GetMovement((adjx, adjy));
                                var nextCell = lines[nexty][nextx];
                                portalName = direction == Direction.North || direction == Direction.West
                                    ? "" + nextCell + adjacentCell
                                    : "" + adjacentCell + nextCell;
                                type = portalName switch
                                {
                                    "AA" => CellType.Entrance,
                                    "ZZ" => CellType.Exit,
                                    _ => CellType.Portal,
                                };

                                if (type == CellType.Entrance) entrance = (x, y);

                                if (type == CellType.Portal)
                                {
                                    isOuterPortal = (x == 2 || y == 2 || x == line.Length - 3 || y == lines.Count - 3);
                                    if (portals.TryGetValue(portalName, out ((int x, int y) portal1, (int x, int y) portal2) value))
                                    {
                                        portals[portalName] = value with { portal2 = (x, y) };
                                    }
                                    else
                                    {
                                        portals[portalName] = ((x, y), (0, 0));
                                    }
                                }
                            }
                        }
                    }
                    row.Add(new Cell
                    {
                        Type = type,
                        PortalName = portalName,
                        IsOuterPortal = isOuterPortal,
                    });
                }
                grid.Add(row);
            }

            foreach (var (portal1, portal2) in portals.Values)
            {
                grid[portal1.y][portal1.x].MatchingPortalLocation = portal2;
                grid[portal2.y][portal2.x].MatchingPortalLocation = portal1;
            }

            return new Grid
            {
                Cells = grid,
                Entrance = entrance,
            };
        }

        private class Grid
        {
            public (int x, int y) Entrance { get; set; }
            public List<List<Cell>> Cells { get; set; } = [];

            public Cell Get((int x, int y) position)
            {
                return Cells[position.y][position.x];
            }
        }

        private class Cell
        {
            public CellType Type { get; set; }
            public string PortalName { get; set; } = "";
            public (int x, int y) MatchingPortalLocation { get; set; }
            public bool IsOuterPortal { get; set; }
        }

        private enum CellType
        {
            Open,
            Wall,
            Portal,
            Entrance,
            Exit,
        }
    }
}
