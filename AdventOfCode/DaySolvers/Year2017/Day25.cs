namespace AdventOfCode.Year2017
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var numSteps = 12994925;
            var onePositions = new HashSet<int>();
            var currentPosition = 0;
            var currentState = 'a';
            var states = new Dictionary<char, ((int write, int move, char next) zero, (int write, int move, char next) one)>
            {
                { 'a', ((1, 1, 'b'), (0, -1, 'f')) },
                { 'b', ((0, 1, 'c'), (0, 1, 'd')) },
                { 'c', ((1, -1, 'd'), (1, 1, 'e')) },
                { 'd', ((0, -1, 'e'), (0, -1, 'd')) },
                { 'e', ((0, 1, 'a'), (1, 1, 'c')) },
                { 'f', ((1, -1, 'a'), (1, 1, 'a')) },
            };

            for (var i = 0; i < numSteps; i++)
            {
                var (zero, one) = states[currentState];
                if (onePositions.Contains(currentPosition))
                {
                    if (one.write == 0) onePositions.Remove(currentPosition);
                    currentPosition += one.move;
                    currentState = one.next;
                }
                else
                {
                    if (zero.write == 1) onePositions.Add(currentPosition);
                    currentPosition += zero.move;
                    currentState = zero.next;
                }
            }

            var expectedResult = 2846;
            var result = onePositions.Count;
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
