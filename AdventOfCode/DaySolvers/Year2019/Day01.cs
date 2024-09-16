
namespace AdventOfCode.Year2019
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.Sum(l => (int.Parse(l) / 3) - 2);
        }

        public object HardSolution(IList<string> lines)
        {
            return lines.Sum(l => GetRequiredFuel(int.Parse(l)));
        }

        private static long GetRequiredFuel(int i)
        {
            var fuel = i / 3 - 2;
            if (fuel <= 0) return 0;
            return fuel + GetRequiredFuel(fuel);
        }
    }
}
