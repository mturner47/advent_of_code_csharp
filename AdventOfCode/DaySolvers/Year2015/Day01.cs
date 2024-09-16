namespace AdventOfCode.Year2015
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var upCount = lines[0].Count(c => c == '(');
            var downCount = lines[0].Count(c => c == ')');
            var expectedResult = 74;
            var result = upCount - downCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var line = lines[0];
            var currentFloor = 0;
            var targetIndex = 0;
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '(') currentFloor++;
                else currentFloor--;

                if (currentFloor == -1)
                {
                    targetIndex = i + 1;
                    break;
                }
            }
            var expectedResult = 1795;
            var result = targetIndex;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
