using Helpers.Helpers;

namespace AdventOfCode.Year2017
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = double.Parse(lines[0]);
            var smallerSquare = Math.Floor(Math.Sqrt(input));
            if (smallerSquare % 2 == 0) smallerSquare--;
            var cellsPriorToRelevantSquare = Math.Pow(smallerSquare, 2);
            var lengthOfRelevantSquare = smallerSquare += 1;
            var halfWay = lengthOfRelevantSquare / 2;
            var sidesPassed = Math.Floor((input - cellsPriorToRelevantSquare) / lengthOfRelevantSquare);
            var cellsPriorToRelevantSide = cellsPriorToRelevantSquare + lengthOfRelevantSquare * sidesPassed;
            var cellsOnRelevantSide = input - cellsPriorToRelevantSide;
            var distanceFromCenter = Math.Abs(cellsOnRelevantSide - halfWay);
            var answer = distanceFromCenter + halfWay;

            var expectedResult = 475;
            var result = answer;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var threshold = int.Parse(lines[0]);

            var expectedResult = 279138;
            var result = SolveHard(threshold);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int SolveHard(int threshold)
        {

            var dict = new Dictionary<(int x, int y), int>();
            var currentPosition = (x: 0, y: 0);
            dict.Add(currentPosition, 1);
            var currentDirection = Direction.East;

            while (true)
            {
                currentPosition = DirectionExtensions.GetMovement(currentDirection, currentPosition);
                var sum = 0;
                var surroundingSquares = DirectionExtensions.GetAllMovements(currentPosition, true);
                foreach (var square in surroundingSquares)
                {
                    if (dict.ContainsKey(square)) sum += dict[square];
                }

                if (sum > threshold) return sum;
                dict[currentPosition] = sum;
                var cellToLeft = currentDirection.GetCCW().GetMovement(currentPosition);
                if (!dict.ContainsKey(cellToLeft)) currentDirection = currentDirection.GetCCW();
            }

        }
    }
}
