namespace AdventOfCode.DaySolvers.Year2024
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var stones = lines[0].Split(" ").Select(double.Parse).GroupBy(d => d).ToDictionary(d => d.Key, d => (double)d.Count());
            var expectedResult = 186424;
            var result = Blink(stones, 25).Values.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var stones = lines[0].Split(" ").Select(double.Parse).GroupBy(d => d).ToDictionary(d => d.Key, d => (double)d.Count());
            var expectedResult = 219838428124832;
            var result = Blink(stones, 75).Values.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Dictionary<double, double> Blink(Dictionary<double, double> stones, int numBlinks)
        {
            for (var i = 0; i < numBlinks; i++)
            {
                var newStones = new Dictionary<double, double>();
                foreach (var stone in stones)
                {
                    var stonesToIncrement = new List<double>();
                    if (stone.Key == 0) stonesToIncrement.Add(1);
                    else
                    {
                        var stoneString = stone.Key.ToString();
                        if (stoneString.Length % 2 == 0)
                        {
                            stonesToIncrement.Add(double.Parse(stoneString[..(stoneString.Length / 2)]));
                            stonesToIncrement.Add(double.Parse(stoneString[(stoneString.Length / 2)..]));
                        }
                        else stonesToIncrement.Add(stone.Key * 2024);
                    }

                    foreach (var stoneToIncrement in stonesToIncrement)
                    {
                        if (!newStones.TryGetValue(stoneToIncrement, out double value)) newStones.Add(stoneToIncrement, stone.Value);
                        else newStones[stoneToIncrement] = value + stone.Value;
                    }
                }
                stones = newStones;
            }
            return stones;
        }
    }
}
