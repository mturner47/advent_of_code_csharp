namespace AdventOfCode.Year2022
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (grid, startingCoordinates, endingCoordinates) = ConvertToGrid(lines, false);
            var uncheckedCellCoordinates = new List<(int, int)> { startingCoordinates };
            while (uncheckedCellCoordinates.Any())
            {
                var uncheckedCellCoordinate = uncheckedCellCoordinates.First();
                uncheckedCellCoordinates.Remove(uncheckedCellCoordinate);
                var cell = grid[uncheckedCellCoordinate];
                var cellHeight = cell.Height;
                var (x, y) = cell.Coordinates;
                var pathLength = cell.ShortestFoundPath!.Count;

                var adjacentCoordinates = new List<(int, int)> { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1) };
                foreach (var adjacentCoord in adjacentCoordinates)
                {
                    if (grid.ContainsKey(adjacentCoord))
                    {
                        var adjacentCell = grid[adjacentCoord];
                        if (adjacentCell.Height <= cellHeight + 1)
                        {
                            if (adjacentCell.ShortestFoundPath == null || adjacentCell.ShortestFoundPath.Count > pathLength + 1)
                            {
                                adjacentCell.ShortestFoundPath = cell.ShortestFoundPath.Concat(new List<(int, int)> { adjacentCoord }).ToList();
                                uncheckedCellCoordinates.Add(adjacentCoord);
                            }
                        }
                    }
                }
            }

            return grid[endingCoordinates].ShortestFoundPath!.Count - 1;
        }

        public object HardSolution(IList<string> lines)
        {
            var (grid, startingCoordinates, endingCoordinates) = ConvertToGrid(lines, true);
            var uncheckedCellCoordinates = new List<(int, int)> { startingCoordinates };
            while (uncheckedCellCoordinates.Any())
            {
                var uncheckedCellCoordinate = uncheckedCellCoordinates.First();
                uncheckedCellCoordinates.Remove(uncheckedCellCoordinate);
                var cell = grid[uncheckedCellCoordinate];
                var cellHeight = cell.Height;
                var (x, y) = cell.Coordinates;
                var pathLength = cell.ShortestFoundPath!.Count;

                var adjacentCoordinates = new List<(int, int)> { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1) };
                foreach (var adjacentCoord in adjacentCoordinates)
                {
                    if (grid.ContainsKey(adjacentCoord))
                    {
                        var adjacentCell = grid[adjacentCoord];
                        if (adjacentCell.Height >= cellHeight - 1)
                        {
                            if (adjacentCell.ShortestFoundPath == null || adjacentCell.ShortestFoundPath.Count > pathLength + 1)
                            {
                                adjacentCell.ShortestFoundPath = cell.ShortestFoundPath.Concat(new List<(int, int)> { adjacentCoord }).ToList();
                                uncheckedCellCoordinates.Add(adjacentCoord);
                            }
                        }
                    }
                }
            }

            var minHeight = grid.Values.Min(c => c.Height);
            return grid.Values.Where(c => c.Height == minHeight && c.ShortestFoundPath != null).OrderBy(c => c.ShortestFoundPath!.Count).First().ShortestFoundPath!.Count - 1;
        }

        private static (Dictionary<(int, int), Cell>, (int, int), (int, int)) ConvertToGrid(IList<string> lines, bool shouldReverse)
        {
            var grid = new Dictionary<(int, int), Cell>();
            (int, int) startingCoordinates = (0, 0);
            (int, int) endingCoordinates = (0, 0);
            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var c = lines[y][x];
                    var coordinates = (x, y);
                    var isStartingCell = c == 'S';
                    if (isStartingCell)
                    {
                        c = 'a';
                        if (!shouldReverse)
                        {
                            startingCoordinates = coordinates;
                        }
                        else
                        {
                            isStartingCell = false;
                        }
                    }
                    var isEndingCell = c == 'E';
                    if (isEndingCell)
                    {
                        c = 'z';
                        if (shouldReverse)
                        {
                            startingCoordinates = coordinates;
                            isStartingCell = true;
                            isEndingCell = false;
                        }
                        else
                        {
                            endingCoordinates = coordinates;
                        }
                    }

                    var height = (int)c;
                    var cell = new Cell
                    {
                        Coordinates = coordinates,
                        Height = height,
                        ShortestFoundPath = isStartingCell ? new List<(int, int)> { coordinates } : null,
                        IsStartingCell = isStartingCell,
                        IsEndingCell = isEndingCell,
                    };
                    grid[coordinates] = cell;
                }
            }
            return (grid, startingCoordinates, endingCoordinates);
        }

        private class Cell
        {
            public (int, int) Coordinates { get; set; }
            public int Height { get; set; }
            public List<(int, int)>? ShortestFoundPath { get; set; }
            public bool IsStartingCell { get; set; }
            public bool IsEndingCell { get; set; }
        }
    }
}
