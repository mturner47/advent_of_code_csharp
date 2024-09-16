namespace AdventOfCode.Year2017
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var genA = 289L;
            var genB = 629L;

            var matchCount = 0;
            for (var i = 0; i < 40_000_000; i++)
            {
                genA = (genA * 16807) % 2147483647;
                genB = (genB * 48271) % 2147483647;
                var genAString = ConvertToBitString(genA);
                var genBString = ConvertToBitString(genB);
                if (genAString[^16..] == genBString[^16..]) matchCount++;
            }
            var expectedResult = 638;
            var result = matchCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var genA = 289L;
            var mulA = 16807;
            var divA = 4;
            var genB = 629L;
            var mulB = 48271;
            var divB = 8;

            var aValues = GetValues(genA, mulA, divA);
            var bValues = GetValues(genB, mulB, divB);
            var pairs = aValues.Zip(bValues).Take(5_000_000);

            var expectedResult = 638;
            var result = pairs.Count(p => p.First == p.Second); ;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static IEnumerable<string> GetValues(long value, int multiplier, int divisor)
        {
            while (true)
            {
                value = (value * multiplier) % 2147483647;
                if (value % divisor == 0) yield return ConvertToBitString(value)[^16..];
            }
        }

        private static string ConvertToBitString(long value)
        {
            return Convert.ToString(value, 2).PadLeft(32, '0');
        }
    }
}
