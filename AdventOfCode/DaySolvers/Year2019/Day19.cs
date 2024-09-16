using AdventOfCode.DaySolvers.Year2019;

namespace AdventOfCode.Year2019
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var count = 0L;
            for (var y = 0; y < 50; y++)
            {
                for (var x = 0; x < 50; x++)
                {
                    var output = IntCodeComputer.ParseAndRunProgram(lines, [x,y]).Outputs[0];
                    count += output;
                }
            }

            var expectedResult = 162;
            var result = count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var (x, y) = FindClosestPoint(lines);
            var expectedResult = 13021056;
            var result = x * 10000 + y;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int x, int y) FindClosestPoint(IList<string> lines)
        {
            var minXFound = 0;
            for (var y = 99; y < int.MaxValue; y++)
            {
                for (var x = minXFound; x < int.MaxValue; x++)
                {
                    if (IntCodeComputer.ParseAndRunProgram(lines, [x, y]).Outputs[0] == 1)
                    {
                        var oppositePoint = (x:x + 99, y:y - 99);
                        minXFound = x;
                        if (oppositePoint.y >= 0)
                        {
                            if (IntCodeComputer.ParseAndRunProgram(lines, [oppositePoint.x, oppositePoint.y]).Outputs[0] == 1)
                            {
                                return (x, oppositePoint.y);
                            }
                        }
                        break;
                    }
                }
            }
            throw new Exception("Not found");
        }
    }
}
