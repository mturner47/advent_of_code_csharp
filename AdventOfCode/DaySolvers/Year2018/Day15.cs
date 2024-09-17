using Helpers.Helpers;
using System.Text;

namespace AdventOfCode.Year2018
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var units = new List<Unit>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if (c == 'G' || c == 'E')
                    {
                        units.Add(new Unit { Type = c, Position = (x, y) });
                    }
                }
            }

            var grid = lines.Select(l => l.Replace("G", ".").Replace("E", ".")).ToList();

            while (true)
            {
                var currentUnit = units.OrderBy(u => u.TurnsTaken).ThenBy(u => u.Position.y).ThenBy(u => u.Position.x).First();
                currentUnit.TurnsTaken++;
                var opposingType = currentUnit.Type == 'G' ? 'E' : 'G';
                var opposingUnits = units.Where(u => u.Type == opposingType).ToList();
                if (opposingUnits.Count == 0) break;

                var adjacentSquares = opposingUnits.SelectMany(u =>
                {
                    return DirectionExtensions.GetAllMovements(u.Position)
                        .Where(pos => pos == currentUnit.Position || grid[pos.y][pos.x] == '.');
                }).ToList();

                if (adjacentSquares.Count == 0) continue;

                // Handle Movement
                if (!adjacentSquares.Any(s => s == currentUnit.Position)) // No need to move, already adjacent to an enemy
                {
                    var allReachableSquares = FillPositions(currentUnit.Position, grid, units)
                        .Where(kvp => adjacentSquares.Contains(kvp.Key))
                        .ToList();

                    if (allReachableSquares.Count == 0) continue;

                    var minDistance = allReachableSquares.Min(kvp => kvp.Value);
                    var closestReachableSquare = allReachableSquares.Where(kvp => kvp.Value == minDistance)
                        .Select(kvp => kvp.Key)
                        .OrderBy(pos => pos.y)
                        .ThenBy(pos => pos.x)
                        .First();

                    currentUnit.Position = DetermineNextMovement(currentUnit.Position, closestReachableSquare, grid, units);
                }

                // Damage
                var attackSquares = DirectionExtensions.GetAllMovements(currentUnit.Position);
                var adjacentEnemy = opposingUnits.Where(u => attackSquares.Contains(u.Position))
                    .OrderBy(u => u.Health)
                    .ThenBy(u => u.Position.y)
                    .ThenBy(u => u.Position.x)
                    .FirstOrDefault();

                if (adjacentEnemy != null)
                {
                    adjacentEnemy.Health -= currentUnit.Attack;
                    if (adjacentEnemy.Health < 0) units.Remove(adjacentEnemy);
                }
            }

            var expectedResult = 181522;
            var result = units.Min(u => u.TurnsTaken) * units.Sum(u => u.Health);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var units = new List<Unit>();
            var shouldLog = false;
            for (var atk = 4; atk < 200; atk++)
            {
                if (shouldLog) Console.WriteLine($"Starting Elf Attack={atk}");
                units.Clear();
                for (var y = 0; y < lines.Count; y++)
                {
                    var line = lines[y];
                    for (var x = 0; x < line.Length; x++)
                    {
                        var c = line[x];
                        if (c == 'G' || c == 'E')
                        {
                            var attack = c == 'E' ? atk : 3;
                            units.Add(new Unit { Type = c, Position = (x, y), Attack = attack });
                        }
                    }
                }

                var grid = lines.Select(l => l.Replace("G", ".").Replace("E", ".")).ToList();
                var elfDied = false;
                while (true)
                {
                    var currentUnit = units.OrderBy(u => u.TurnsTaken).ThenBy(u => u.Position.y).ThenBy(u => u.Position.x).First();
                    var opposingType = currentUnit.Type == 'G' ? 'E' : 'G';
                    var opposingUnits = units.Where(u => u.Type == opposingType).ToList();
                    if (opposingUnits.Count == 0) break;
                    currentUnit.TurnsTaken++;

                    var adjacentSquares = opposingUnits.SelectMany(u =>
                    {
                        return DirectionExtensions.GetAllMovements(u.Position)
                            .Where(pos => pos == currentUnit.Position || grid[pos.y][pos.x] == '.');
                    }).ToList();

                    if (adjacentSquares.Count == 0) continue;

                    // Handle Movement
                    if (!adjacentSquares.Any(s => s == currentUnit.Position)) // No need to move, already adjacent to an enemy
                    {
                        var allReachableSquares = FillPositions(currentUnit.Position, grid, units)
                            .Where(kvp => adjacentSquares.Contains(kvp.Key))
                            .ToList();

                        if (allReachableSquares.Count == 0) continue;

                        var minDistance = allReachableSquares.Min(kvp => kvp.Value);
                        var closestReachableSquare = allReachableSquares.Where(kvp => kvp.Value == minDistance)
                            .Select(kvp => kvp.Key)
                            .OrderBy(pos => pos.y)
                            .ThenBy(pos => pos.x)
                            .First();

                        var nextPosition = DetermineNextMovement(currentUnit.Position, closestReachableSquare, grid, units);
                        if (shouldLog) Console.WriteLine($"{currentUnit.Type} moved from ({currentUnit.Position.x},{currentUnit.Position.y}) to ({nextPosition.x},{nextPosition.y})");
                        currentUnit.Position = nextPosition;
                    }

                    // Damage
                    var attackSquares = DirectionExtensions.GetAllMovements(currentUnit.Position);
                    var adjacentEnemy = opposingUnits.Where(u => attackSquares.Contains(u.Position))
                        .OrderBy(u => u.Health)
                        .ThenBy(u => u.Position.y)
                        .ThenBy(u => u.Position.x)
                        .FirstOrDefault();

                    if (adjacentEnemy != null)
                    {
                        if (shouldLog) Console.WriteLine($"{currentUnit.Type} at ({currentUnit.Position.x},{currentUnit.Position.y}) attacked {adjacentEnemy.Type} at ({adjacentEnemy.Position.x},{adjacentEnemy.Position.y}) bringing their health from {adjacentEnemy.Health} to {adjacentEnemy.Health - currentUnit.Attack}.");
                        adjacentEnemy.Health -= currentUnit.Attack;
                        if (adjacentEnemy.Health <= 0)
                        {
                            if (shouldLog) Console.WriteLine($"{adjacentEnemy.Type} at ({adjacentEnemy.Position.x},{adjacentEnemy.Position.y}) is killed.");
                            if (adjacentEnemy.Type == 'E')
                            {
                                elfDied = true;
                                break;
                            }

                            units.Remove(adjacentEnemy);
                        }
                    }
                    if (shouldLog) DrawBattlefield(grid, units);
                }
                if (!elfDied) break;
            }

            var expectedResult = 68324;
            var result = units.Min(u => u.TurnsTaken) * units.Sum(u => u.Health);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Dictionary<(int x, int y), int> FillPositions((int x, int y) position, List<string> grid, List<Unit> units)
        {
            var knownDistances = new Dictionary<(int x, int y), int>();
            var unitPositions = units.Select(u => u.Position).ToList();
            knownDistances.Add(position, 0);
            var cellsToCheck = new Queue<((int x, int y) position, int distance)>();
            cellsToCheck.Enqueue((position, 0));
            while (cellsToCheck.Count > 0)
            {
                var (cell, distance) = cellsToCheck.Dequeue();
                var newDistance = distance + 1;
                foreach (var adjacentCell in DirectionExtensions.GetAllMovements(cell))
                {
                    if (unitPositions.Contains(adjacentCell)) continue;
                    if (knownDistances.ContainsKey(adjacentCell)) continue;
                    if (grid[adjacentCell.y][adjacentCell.x] == '#') continue;
                    knownDistances.Add(adjacentCell, newDistance);
                    cellsToCheck.Enqueue((adjacentCell, newDistance));
                }
            }
            return knownDistances;
        }

        private static (int x, int y) DetermineNextMovement((int x, int y) position, (int x, int y) target, List<string> grid, List<Unit> units)
        {
            var unitPositions = units.Select(u => u.Position).ToList();
            var startingNeighbors = DirectionExtensions.GetAllMovements(position).Where(p => grid[p.y][p.x] != '#' && !unitPositions.Contains(p)).ToList();
            if (startingNeighbors.Count == 1)
            {
                return startingNeighbors[0];
            }

            var seenOnPaths = startingNeighbors.ToDictionary(sn => sn, sn => new List<(int x, int y)> { position, sn });

            var queue = new PriorityQueue<((int x, int y) currentPos, (int x, int y) startingNeighbor, int distance), long>();
            foreach (var neighbor in startingNeighbors)
            {
                queue.Enqueue((neighbor, neighbor, 1), 1 + MathHelpers.ManhattanDistance(neighbor, target));
            }

            var possibleStartingPositions = new List<(int x, int y)>();
            var shortestPath = int.MaxValue;
            while (queue.Count > 0)
            {
                var (currentPosition, sn, distance) = queue.Dequeue();
                if (currentPosition == target)
                {
                    possibleStartingPositions.Add(sn);
                    shortestPath = distance;
                    continue;
                }

                if (shortestPath < MathHelpers.ManhattanDistance(currentPosition, target) + distance) break;

                seenOnPaths[sn].Add(currentPosition);

                foreach (var nextPosition in DirectionExtensions.GetAllMovements(currentPosition))
                {
                    if (nextPosition == position) continue;
                    if (seenOnPaths[sn].Contains(nextPosition)) continue;
                    if (grid[nextPosition.y][nextPosition.x] == '#') continue;
                    if (unitPositions.Contains(nextPosition)) continue;
                    var newDistance = distance + 1;
                    queue.Enqueue((nextPosition, sn, newDistance), newDistance + MathHelpers.ManhattanDistance(nextPosition, target));
                }
            }

            return possibleStartingPositions.OrderBy(p => p.y).ThenBy(p => p.x).First();
        }

        private static void DrawBattlefield(List<string> grid, List<Unit> units)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < grid.Count; y++)
            {
                var line = grid[y];
                var unitsOnLine = new List<string>();
                for (var x = 0; x < line.Length; x++)
                {
                    var unitAtPosition = units.FirstOrDefault(u => u.Position == (x, y));
                    if (unitAtPosition != null)
                    {
                        sb.Append(unitAtPosition.Type);
                        unitsOnLine.Add($"{unitAtPosition.Type}({unitAtPosition.Health})");
                    }
                    else
                    {
                        sb.Append(line[x]);
                    }
                }
                if (unitsOnLine.Count > 0) sb.Append("    " + string.Join(", ", unitsOnLine));
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }

        private class Unit
        {
            public char Type;
            public int Health = 200;
            public int Attack = 3;
            public (int x, int y) Position;
            public int TurnsTaken = 0;
        }
    }
}
