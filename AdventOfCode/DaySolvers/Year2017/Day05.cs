namespace AdventOfCode.Year2017
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var steps = lines.Select(int.Parse).ToList();
            var currentIndex = 0;
            var numActions = 0;
            while (currentIndex < steps.Count)
            {
                numActions++;
                var nextIndex = currentIndex + steps[currentIndex];
                steps[currentIndex]++;
                currentIndex = nextIndex;
            }
            var expectedResult = 351282;
            var result = numActions;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var steps = lines.Select(int.Parse).ToList();
            var currentIndex = 0;
            var numActions = 0;
            while (currentIndex < steps.Count)
            {
                numActions++;
                var nextIndex = currentIndex + steps[currentIndex];
                if (steps[currentIndex] >= 3) steps[currentIndex]--;
                else steps[currentIndex]++;
                currentIndex = nextIndex;
            }
            var expectedResult = 24568703;
            var result = numActions;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
