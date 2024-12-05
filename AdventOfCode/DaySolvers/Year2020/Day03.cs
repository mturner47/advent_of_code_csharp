namespace AdventOfCode.Year2020
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (x, y) = (0, 0);
            var (sx, sy) = (3, 1);
            var crashCount = 0;

            while (y < lines.Count)
            {
                if (lines[y][x] == '#') crashCount++;
                y += sy;
                x = (x + sx) % lines[0].Length;
            }

            var expectedResult = 268;
            var result = crashCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var slopes = new List<(int x, int y)>
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2),
            };

            var totalCrashCount = 1L;
            foreach (var (sx, sy) in slopes)
            {
                var (x, y) = (0, 0);
                var crashCount = 0;

                while (y < lines.Count)
                {
                    if (lines[y][x] == '#') crashCount++;
                    y += sy;
                    x = (x + sx) % lines[0].Length;
                }
                totalCrashCount *= crashCount;
            }

            var expectedResult = 3093068400;
            var result = totalCrashCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
