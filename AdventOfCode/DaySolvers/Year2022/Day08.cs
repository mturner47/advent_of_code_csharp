namespace AdventOfCode.Year2022
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = ConvertToGrid(lines);
            SetNorthWestVisibility(grid);
            SetSouthEastVisibility(grid);

            return grid.Select(r => r.Count(c => c.IsVisible)).Sum();
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = ConvertToGrid(lines);

            for (var i = 0; i < grid.Count; i++)
            {
                var row = grid[i];
                for (var j = 0; j < row.Count; j++)
                {
                    var cell = row[j];
                    var height = cell.Height;

                    var viewNorth = 0;
                    for (var i2 = i - 1; i2 >= 0; i2--)
                    {
                        viewNorth++;
                        if (grid[i2][j].Height >= height) break;
                    }

                    var viewSouth = 0;
                    for (var i2 = i + 1; i2 < grid.Count; i2++)
                    {
                        viewSouth++;
                        if (grid[i2][j].Height >= height) break;
                    }

                    var viewWest = 0;
                    for (var j2 = j - 1; j2 >= 0; j2--)
                    {
                        viewWest++;
                        if (row[j2].Height >= height) break;
                    }

                    var viewEast = 0;
                    for (var j2 = j + 1; j2 < row.Count; j2++)
                    {
                        viewEast++;
                        if (row[j2].Height >= height) break;
                    }

                    cell.ScenicScore = viewNorth * viewSouth * viewEast * viewWest;
                }
            }

            return grid.Select(r => r.Max(c => c.ScenicScore)).Max();
        }

        private void SetNorthWestVisibility(List<List<Cell>> grid)
        {
            for (var i = 0; i < grid.Count; i++)
            {
                var row = grid[i];
                for (var j = 0; j < row.Count; j++)
                {
                    var cell = row[j];
                    if (j == 0 || cell.Height > row[j - 1].TallestWest)
                    {
                        cell.IsVisibleFromWest = true;
                        cell.TallestWest = cell.Height;
                    }
                    else
                    {
                        cell.TallestWest = row[j - 1].TallestWest;
                    }

                    if (i == 0 || cell.Height > grid[i - 1][j].TallestNorth)
                    {
                        cell.IsVisibleFromNorth = true;
                        cell.TallestNorth = cell.Height;
                    }
                    else
                    {
                        cell.TallestNorth = grid[i - 1][j].TallestNorth;
                    }
                }
            }
        }

        private void SetSouthEastVisibility(List<List<Cell>> grid)
        {
            for (var i = grid.Count - 1; i >= 0; i--)
            {
                var row = grid[i];
                for (var j = row.Count - 1; j >= 0; j--)
                {
                    var cell = row[j];
                    if (j == row.Count - 1 || cell.Height > row[j + 1].TallestEast)
                    {
                        cell.IsVisibleFromEast = true;
                        cell.TallestEast = cell.Height;
                    }
                    else
                    {
                        cell.TallestEast = row[j + 1].TallestEast;
                    }

                    if (i == grid.Count - 1 || cell.Height > grid[i + 1][j].TallestSouth)
                    {
                        cell.IsVisibleFromSouth = true;
                        cell.TallestSouth = cell.Height;
                    }
                    else
                    {
                        cell.TallestSouth = grid[i + 1][j].TallestSouth;
                    }
                }
            }
        }

        private static List<List<Cell>> ConvertToGrid(IList<string> lines)
        {
            return lines.Select(line => line.Select(c => new Cell
            {
                Height = c - '0',
            }).ToList())
            .ToList();
        }

        private class Cell
        {
            public int Height { get; set; }
            public bool IsVisibleFromNorth { get; set; }
            public bool IsVisibleFromSouth { get; set; }
            public bool IsVisibleFromEast { get; set; }
            public bool IsVisibleFromWest { get; set; }
            public int TallestNorth { get; set; }
            public int TallestSouth { get; set; }
            public int TallestEast { get; set; }
            public int TallestWest { get; set; }
            public bool IsVisible => IsVisibleFromNorth || IsVisibleFromSouth || IsVisibleFromEast || IsVisibleFromWest;
            public double ScenicScore { get; set; }
        }
    }
}
