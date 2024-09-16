namespace AdventOfCode.Year2015
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var goalCount = double.Parse(lines.First()) / 10;
            var houses = new Dictionary<double, double>();
            for (var i = 1; i < goalCount; i++)
            {
                for (var j = i; j < goalCount; j += i)
                {
                    if (!houses.TryGetValue(j, out var currentHouseCount)) currentHouseCount = 0d;
                    houses[j] = currentHouseCount + i;
                }
            }

            var expectedResult = 786240d;
            var result = houses.Where(kvp => kvp.Value >= goalCount).Min(kvp => kvp.Key);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var goalCount = int.Parse(lines.First()) / 11;
            var houses = new int[goalCount];
            var currentMin = goalCount;
            for (var i = 1; i < goalCount; i++)
            {
                for (var j = i; j < i + 50*i && j + i < goalCount; j += i)
                {
                    var newSum = houses[j] + i;
                    houses[j] = newSum;
                    if (newSum > goalCount && currentMin > j)
                    {
                        currentMin = j;
                    }
                }
            }

            var expectedResult = 786240d;

            var result = currentMin;

            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
