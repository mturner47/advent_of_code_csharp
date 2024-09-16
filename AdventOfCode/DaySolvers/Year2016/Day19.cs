namespace AdventOfCode.Year2016
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var numElves = int.Parse(lines[0]);
            var activeElves = Enumerable.Range(1, numElves).ToList();
            while(activeElves.Count > 1)
            {
                var activeElfCount = activeElves.Count;
                activeElves = activeElves.Where((x, i) => i%2 == 0).ToList();
                if (activeElfCount % 2 == 1) activeElves.RemoveAt(0);
            }
            var expectedResult = 1834903;
            var result = activeElves[0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var numElves = int.Parse(lines[0]);
            var i = 1;
            while (i*3 < numElves)
            {
                i *= 3;
            }

            var expectedResult = 1420280;
            var result = numElves - i;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
