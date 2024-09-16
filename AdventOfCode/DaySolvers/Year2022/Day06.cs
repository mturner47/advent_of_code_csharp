namespace AdventOfCode.Year2022
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var line = lines[0];
            for (var i = 3; i < line.Length; i++)
            {
                var items = line.Skip(i - 3).Take(4).Distinct();
                if (items.Count() == 4)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public object HardSolution(IList<string> lines)
        {
            var line = lines[0];
            for (var i = 13; i < line.Length; i++)
            {
                var items = line.Skip(i - 13).Take(14).Distinct();
                if (items.Count() == 14)
                {
                    return i + 1;
                }
            }
            return 0;
        }
    }
}
