namespace AdventOfCode.Year2016
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            List<int> sizes = [17, 7, 19, 5, 3, 13];
            List <int> startingPoints = [1, 0, 2, 0, 0, 5];

            var expectedResult = 317371;
            var result = Solve(sizes, startingPoints);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            List<int> sizes = [17, 7, 19, 5, 3, 13, 11];
            List<int> startingPoints = [1, 0, 2, 0, 0, 5, 0];

            var expectedResult = 2080951;
            var result = Solve(sizes, startingPoints);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(List<int> sizes, List<int> startingPoints)
        {
            var t = 0;
            while (true)
            {
                t++;
                var allZero = true;
                for (var i = 0; i < sizes.Count; i++)
                {
                    if ((t + i + 1 + startingPoints[i]) % sizes[i] != 0)
                    {
                        allZero = false;
                        break;
                    }
                }
                if (allZero) return t;
            }
        }
    }
}
