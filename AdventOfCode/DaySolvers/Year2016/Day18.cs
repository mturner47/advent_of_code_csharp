namespace AdventOfCode.Year2016
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            List<string> grid = [lines[0]];
            for (var i = 1; i < 40; i++)
            {
                var priorRow = grid[i - 1];
                var newRow = "";
                for (var j = 0; j < priorRow.Length; j++)
                {
                    var leftSafe = j == 0 || priorRow[j - 1] == '.';
                    var centerSafe = priorRow[j] == '.';
                    var rightSafe = j == priorRow.Length - 1 || priorRow[j + 1] == '.';

                    var newChar = leftSafe == rightSafe ? '.' : '^';
                    newRow += newChar;
                }
                grid.Add(newRow);
            }
            var expectedResult = 1982;
            var result = grid.Sum(l => l.Count(c => c == '.'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            List<string> grid = [lines[0]];
            for (var i = 1; i < 400000; i++)
            {
                var priorRow = grid[i - 1];
                var newRow = "";
                for (var j = 0; j < priorRow.Length; j++)
                {
                    var leftSafe = j == 0 || priorRow[j - 1] == '.';
                    var centerSafe = priorRow[j] == '.';
                    var rightSafe = j == priorRow.Length - 1 || priorRow[j + 1] == '.';

                    var newChar = leftSafe == rightSafe ? '.' : '^';
                    newRow += newChar;
                }
                grid.Add(newRow);
            }
            var expectedResult = 20005203;
            var result = grid.Sum(l => l.Count(c => c == '.'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
