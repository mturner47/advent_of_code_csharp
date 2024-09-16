namespace AdventOfCode.Year2022
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = ParseInput(lines);
            var maxDepth = grid.Keys.Select(x => x.Item2).Max();

            bool isDone = false;
            while(true)
            {
                var currentCoordinates = (500, 0);
                while (true)
                {
                    var (x, y) = currentCoordinates;
                    if (y >= maxDepth)
                    {
                        isDone = true;
                        break;
                    }
                    if (grid.ContainsKey((x, y + 1)))
                    {
                        if (!grid.ContainsKey((x - 1, y + 1)))
                        {
                            currentCoordinates = (x - 1, y + 1);
                        }
                        else if (!grid.ContainsKey((x + 1, y + 1)))
                        {
                            currentCoordinates = (x + 1, y + 1);
                        }
                        else
                        {
                            grid.Add(currentCoordinates, Cell.Sand);
                            break;
                        }
                    }
                    else
                    {
                        currentCoordinates = (x, y + 1);
                    }
                }
                if (isDone) break;
            }

            return grid.Values.Where(x => x == Cell.Sand).Count();
        }
        public object HardSolution(IList<string> lines)
        {
            var grid = ParseInput(lines);
            var maxDepth = grid.Keys.Select(x => x.Item2).Max();

            bool isDone = false;
            while (true)
            {
                var currentCoordinates = (500, 0);
                while (true)
                {
                    var (x, y) = currentCoordinates;
                    if (y == maxDepth + 1)
                    {
                        grid.Add(currentCoordinates, Cell.Sand);
                        break;
                    }
                    if (grid.ContainsKey((x, y + 1)))
                    {
                        if (!grid.ContainsKey((x - 1, y + 1)))
                        {
                            currentCoordinates = (x - 1, y + 1);
                        }
                        else if (!grid.ContainsKey((x + 1, y + 1)))
                        {
                            currentCoordinates = (x + 1, y + 1);
                        }
                        else
                        {
                            grid.Add(currentCoordinates, Cell.Sand);
                            if ((x, y) == (500, 0)) isDone = true;
                            break;
                        }
                    }
                    else
                    {
                        currentCoordinates = (x, y + 1);
                    }
                }
                if (isDone) break;
            }

            return grid.Values.Where(x => x == Cell.Sand).Count();
        }

        private static IDictionary<(int, int), Cell> ParseInput(IList<string> lines)
        {
            var grid = new Dictionary<(int, int), Cell>();
            foreach (var line in lines)
            {
                var points = line.Split(" -> ").Select(p =>
                {
                    var parts = p.Split(",");
                    return (int.Parse(parts[0]), int.Parse(parts[1]));
                }).ToList();

                for (var i = 0; i < points.Count - 1; i++)
                {
                    var (x1, y1) = points[i];
                    var (x2, y2) = points[i + 1];

                    if (x1 < x2)
                    {
                        for (var x = x1; x <= x2; x++)
                        {
                            var key = (x, y1);
                            if (!grid.ContainsKey(key)) grid.Add(key, Cell.Rock);
                        }
                    }

                    if (x2 < x1)
                    {
                        for (var x = x2; x <= x1; x++)
                        {
                            var key = (x, y1);
                            if (!grid.ContainsKey(key)) grid.Add(key, Cell.Rock);
                        }
                    }

                    if (y1 < y2)
                    {
                        for (var y = y1; y <= y2; y++)
                        {
                            var key = (x1, y);
                            if (!grid.ContainsKey(key)) grid.Add(key, Cell.Rock);
                        }
                    }

                    if (y2 < y1)
                    {
                        for (var y = y2; y <= y1; y++)
                        {
                            var key = (x1, y);
                            if (!grid.ContainsKey(key)) grid.Add(key, Cell.Rock);
                        }
                    }
                }
            }
            return grid;
        }

        private enum Cell
        {
            Sand,
            Rock,
        }
    }
}
