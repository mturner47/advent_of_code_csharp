namespace AdventOfCode.Year2021
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(c => new Node(char.GetNumericValue(c), null)).ToList()).ToList();
            return CalculatePath(grid);
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(c => new Node(char.GetNumericValue(c), null)).ToList()).ToList();
            grid = ExpandGrid(grid);

            return CalculatePath(grid);
        }

        private static double CalculatePath(List<List<Node>> grid)
        {
            var maxX = grid.Count;
            var maxY = grid[0].Count;
            grid[0][0] = grid[0][0] with { TotalCost = 0 };
            var hasChanged = true;

            while (hasChanged)
            {
                hasChanged = false;
                for (var i = 0; i < grid.Count; i++)
                {
                    for (var j = 0; j < grid[i].Count; j++)
                    {
                        var possibleCosts = new List<double>();
                        var (value, totalCost) = grid[i][j];
                        if (totalCost.HasValue)
                        {
                            possibleCosts.Add(totalCost.Value);
                        }

                        var possibleNodes = new[] { (i - 1, j), (i, j - 1), (i + 1, j), (i, j + 1) };
                        foreach (var (i2, j2) in possibleNodes)
                        {
                            if (i2 >= 0 && j2 >= 0 && i2 < maxX && j2 < maxY)
                            {
                                var (value2, totalCost2) = grid[i2][j2];
                                if (totalCost2.HasValue)
                                {
                                    possibleCosts.Add(totalCost2.Value + value);
                                }
                            }
                        }

                        var minTotalCost = possibleCosts.Min();
                        if (minTotalCost != totalCost)
                        {
                            hasChanged = true;
                            grid[i][j] = grid[i][j] with { TotalCost = minTotalCost };
                        }
                    }
                }
            }

            return grid[^1][^1].TotalCost ?? 0d;
        }

        private static List<List<Node>> ExpandGrid(List<List<Node>> grid)
        {
            foreach (var row in grid)
            {
                var colIterations = row.Count * 4;
                for (var i = 0; i < colIterations; i++)
                {
                    row.Add(new Node(row[i].Value == 9 ? 1 : row[i].Value + 1, null));
                }
            }

            var rowIterations = grid.Count * 4;
            for (var i = 0; i < rowIterations; i++)
            {
                var newRow = grid[i].Select(x => new Node(x.Value == 9 ? 1 : x.Value + 1, null)).ToList();
                grid.Add(newRow);
            }

            return grid;
        }

        private record Node(double Value, double? TotalCost);
    }
}
