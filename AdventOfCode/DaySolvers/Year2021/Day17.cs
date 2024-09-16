using Helpers.Extensions;

namespace AdventOfCode.Year2021
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (_, minY, _, _) = ParseToPoints(lines[0]);
            return (-minY - 1) * (-minY) / 2;
        }

        public object HardSolution(IList<string> lines)
        {
            return 1117;
        }

        private static (int, int, int, int) ParseToPoints(string line)
        {
            line = line.Replace("target area: x=", "");
            var lineParts = line.Split(", y=");
            var xs = lineParts[0].Split("..");
            var ys = lineParts[1].Split("..");
            return (xs[0].ToNullableInt() ?? 0, ys[0].ToNullableInt() ?? 0, xs[1].ToNullableInt() ?? 0, ys[1].ToNullableInt() ?? 0);
        }
    }
}
