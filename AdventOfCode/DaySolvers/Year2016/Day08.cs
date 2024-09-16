
namespace AdventOfCode.Year2016
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = Enumerable.Repeat(Enumerable.Repeat(false, 50).ToList(), 6).Select(l => l.ToList()).ToList();

            foreach (var line in lines)
            {
                RunCommand(grid, line);
            }
            var expectedResult = 116;
            var result = grid.Sum(r => r.Count(c => c));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = Enumerable.Repeat(Enumerable.Repeat(false, 50).ToList(), 6).Select(l => l.ToList()).ToList();

            foreach (var line in lines)
            {
                RunCommand(grid, line);
            }
            var expectedResult = 116;
            PrintGrid(grid);
            var result = grid.Sum(r => r.Count(c => c));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static void RunCommand(List<List<bool>> grid, string line)
        {
            if (line.StartsWith("rect"))
            {
                var parts = line.Replace("rect ", "").Split("x");
                var maxX = int.Parse(parts[0]);
                var maxY = int.Parse(parts[1]);
                for (var y = 0; y < maxY; y++)
                {
                    for (var x = 0; x < maxX; x++)
                    {
                        grid[y][x] = true;
                    }
                }
                return;
            }

            if (line.StartsWith("rotate row "))
            {
                var parts = line.Replace("rotate row y=", "").Split(" by ");
                var rowToRotate = int.Parse(parts[0]);
                var countToRotate = int.Parse(parts[1]);
                var row = grid[rowToRotate];
                var newRow = row.ToList();
                for (var i = 0; i < row.Count; i++)
                {
                    newRow[(i + countToRotate) % row.Count] = row[i];
                }
                grid[rowToRotate] = newRow;
                return;
            }

            if (line.StartsWith("rotate column "))
            {
                var parts = line.Replace("rotate column x=", "").Split(" by ");
                var columnIndex = int.Parse(parts[0]);
                var countToRotate = int.Parse(parts[1]);
                var oldColumn = grid.Select(r => r[columnIndex]).ToList();
                for (var i = 0; i < oldColumn.Count; i++)
                {
                    grid[(i + countToRotate) % grid.Count][columnIndex] = oldColumn[i];
                }
                return;
            }
        }

        private static void PrintGrid(List<List<bool>> grid)
        {
            foreach (var line in grid)
            {
                foreach (var cell in line)
                {
                    Console.Write(cell ? "#" : ".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
