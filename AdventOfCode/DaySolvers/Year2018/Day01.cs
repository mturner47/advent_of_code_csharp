namespace AdventOfCode.Year2018
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 484;
            var result = lines.Select(int.Parse).Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var additions = lines.Select(int.Parse).ToList();

            var expectedResult = 367;
            var result = Solve(additions);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(List<int> additions)
        {
            var valuesSeen = new List<int> { 0 };
            var currentValue = 0;

            while (true)
            {
                foreach (var addition in additions)
                {
                    currentValue += addition;
                    if (valuesSeen.Contains(currentValue)) return currentValue;
                    valuesSeen.Add(currentValue);
                }
            }
        }
    }
}
