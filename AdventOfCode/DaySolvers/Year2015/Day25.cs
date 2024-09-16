namespace AdventOfCode.Year2015
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var row = 3010;
            var col = 3019;

            var currentValue = 20151125d;
            var currentRow = 1;
            var currentCol = 1;

            while (true)
            {
                if (currentRow == 1)
                {
                    currentRow = currentCol + 1;
                    currentCol = 1;
                }
                else
                {
                    currentRow--;
                    currentCol++;
                }
                currentValue = (currentValue * 252533) % 33554393;
                if (currentRow == row && currentCol == col) break;
            }

            var expectedResult = 8997277;
            var result = currentValue;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
