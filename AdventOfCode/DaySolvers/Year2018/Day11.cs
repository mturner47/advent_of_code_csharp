namespace AdventOfCode.Year2018
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = int.Parse(lines[0]);
            var powerLevels = GetPowerLevels(input);
            var (x, y, _) = FindBest(powerLevels, 3);

            var expectedResult = "33,54";
            var result = $"{x},{y}";
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var input = int.Parse(lines[0]);
            var powerLevels = GetPowerLevels(input);
            var best = (size:0, x: 0, y: 0, total: 0);

            for (var size = 1; size <= 300; size++)
            {
                var (x, y, total) = FindBest(powerLevels, size);
                if (total < best.total) break;
                else best = (size, x, y, total);
            }

            var expectedResult = "232,289,8";
            var result = $"{best.x},{best.y},{best.size}";
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Dictionary<(int x, int y), int> GetPowerLevels(int input)
        {
            var powerLevels = Enumerable.Range(1, 300).Zip(Enumerable.Range(1, 300))
                .Select(z => (x:z.First, y:z.Second))
                .ToDictionary(p => p, p => 0);

            for (var y = 1; y <= 300; y++)
            {
                for (var x = 1; x <= 300; x++)
                {
                    var rackID = x + 10;
                    var powerLevel = rackID * y;
                    powerLevel += input;
                    powerLevel *= rackID;
                    powerLevel = (powerLevel%1000)/100;
                    powerLevel -= 5;

                    powerLevels[(x, y)] = powerLevel;
                }
            }

            return powerLevels;
        }

        private static (int x, int y, int total) FindBest(Dictionary<(int x, int y), int> powerLevels, int size)
        {
            var best = (x: 0, y: 0, level: 0);

            for (var y = 1; y <= 300 - (size - 1); y++)
            {
                for (var x = 1; x <= 300 - (size - 1); x++)
                {
                    var totalPowerLevel = 0;
                    for (var yi = 0; yi < size; yi++)
                    {
                        for (var xi = 0; xi < size; xi++)
                        {
                            totalPowerLevel += powerLevels[(x + xi, y + yi)];
                        }
                    }
                    if (totalPowerLevel > best.level)
                    {
                        best = (x, y, totalPowerLevel);
                    }
                }
            }

            return best;
        }
    }
}
