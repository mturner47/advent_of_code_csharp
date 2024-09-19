namespace AdventOfCode.Year2018
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 7967233;
            var result = Solve(true);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 16477902;
            var result = Solve(false);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static long Solve(bool onEasy)
        {
            var seenC = new HashSet<long>();
            var lastSeenC = 0L;
            var c = 0L;
            while (true)
            {
                long b = c | 65536;
                c = 10373714;
                while (true)
                {
                    long e = b & 255;
                    c += e;
                    c &= 16777215;
                    c *= 65899;
                    c &= 16777215;
                    if (b < 256)
                    {
                        if (onEasy) return c;
                        if (seenC.Contains(c)) return lastSeenC;

                        seenC.Add(c);
                        lastSeenC = c;
                        break;
                    }
                    e = b / 256;
                    b = e;
                }
            }
        }
    }
}
