using Helpers.Helpers;

namespace AdventOfCode.Year2017
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = "";
            var (result, _) = Solve(lines);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var (_, result) = Solve(lines);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (string seenLetters, int stepsTaken) Solve(IList<string> lines)
        {
            var currentPosition = (lines[0].IndexOf('|'), 0);
            var seenLetters = "";
            var stepsTaken = 1;
            var currentDirection = Direction.South;
            while (true)
            {
                var (x, y) = currentDirection.GetMovement(currentPosition);

                if (x < 0 || x >= lines[0].Length || y < 0 || y >= lines.Count) throw new NotImplementedException();
                var c = lines[y][x];
                switch (c)
                {
                    case '|':
                    case '-':
                        currentPosition = (x, y);
                        stepsTaken++;
                        continue;
                    case ' ':
                        return (seenLetters, stepsTaken);
                    case '+':
                        var (leftx, lefty) = currentDirection.GetCCW().GetMovement((x, y));
                        currentDirection = lines[lefty][leftx] != ' ' ? currentDirection.GetCCW() : currentDirection.GetCW();
                        currentPosition = (x, y);
                        stepsTaken++;
                        continue;
                    default:
                        seenLetters += c;
                        currentPosition = (x, y);
                        stepsTaken++;
                        continue;
                }
            }
        }
    }
}
