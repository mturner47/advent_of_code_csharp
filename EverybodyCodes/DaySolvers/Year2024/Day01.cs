namespace EverybodyCodes.DaySolvers.Year2024
{
    internal class Day01 : IDaySolver
    {
        public object Part1(IList<string> lines)
        {
            var expectedResult = 1323;
            var result = GetTotalPotionsRequired(lines[0], 1);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object Part2(IList<string> lines)
        {
            var expectedResult = 5826;
            var result = GetTotalPotionsRequired(lines[0], 2);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object Part3(IList<string> lines)
        {
            var expectedResult = 28246;
            var result = GetTotalPotionsRequired(lines[0], 3);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int GetTotalPotionsRequired(string source, int groupSize)
        {
            var sum = 0;
            var potionsNeeded = new Dictionary<char, int> { { 'A', 0 }, { 'B', 1 }, { 'C', 3 }, { 'D', 5 } };
            for (var i = 0; i <= source.Length - groupSize + 1; i += groupSize)
            {
                var set = new string(source.Skip(i).Take(groupSize).ToArray());
                var setNoX = set.Replace("x", "");
                sum += setNoX.Sum(c => potionsNeeded[c]) + (setNoX.Length * (setNoX.Length - 1));
            }
            return sum;
        }
    }
}
