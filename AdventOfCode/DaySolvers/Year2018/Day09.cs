namespace AdventOfCode.Year2018
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var numPlayers = 405;
            var maxMarbleValue = 71700;

            var players = Enumerable.Range(0, numPlayers).ToDictionary(p => p, p => new List<long>());

            var currentMarble = new Marble { Value = 0 };
            var currentPlayer = -1;

            for (var i = 1; i <= maxMarbleValue; i++)
            {
                currentPlayer = (currentPlayer + 1) % numPlayers;
                if (i%23 != 0)
                {
                    var prevMarble = currentMarble.Next;
                    var nextMarble = prevMarble.Next;
                    currentMarble = new Marble { Value = i, Next = nextMarble, Prev = prevMarble };
                    prevMarble.Next = currentMarble;
                    nextMarble.Prev = currentMarble;
                }
                else
                {
                    var marbleToRemove = currentMarble.Prev.Prev.Prev.Prev.Prev.Prev.Prev;
                    players[currentPlayer].Add(marbleToRemove.Value);
                    players[currentPlayer].Add(i);

                    var prevMarble = marbleToRemove.Prev;
                    currentMarble = marbleToRemove.Next;
                    prevMarble.Next = currentMarble;
                    currentMarble.Prev = prevMarble;
                }
            }

            var expectedResult = 428690;
            var result = players.Values.Select(v => v.Sum()).Max();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var numPlayers = 405;
            var maxMarbleValue = 71700*100;

            var players = Enumerable.Range(0, numPlayers).ToDictionary(p => p, p => new List<long>());

            var currentMarble = new Marble { Value = 0 };
            var currentPlayer = -1;

            for (var i = 1L; i <= maxMarbleValue; i++)
            {
                currentPlayer = (currentPlayer + 1) % numPlayers;
                if (i % 23 != 0)
                {
                    var prevMarble = currentMarble.Next;
                    var nextMarble = prevMarble.Next;
                    currentMarble = new Marble { Value = i, Next = nextMarble, Prev = prevMarble };
                    prevMarble.Next = currentMarble;
                    nextMarble.Prev = currentMarble;
                }
                else
                {
                    var marbleToRemove = currentMarble.Prev.Prev.Prev.Prev.Prev.Prev.Prev;
                    players[currentPlayer].Add(marbleToRemove.Value);
                    players[currentPlayer].Add(i);

                    var prevMarble = marbleToRemove.Prev;
                    currentMarble = marbleToRemove.Next;
                    prevMarble.Next = currentMarble;
                    currentMarble.Prev = prevMarble;
                }
            }

            var expectedResult = 3628143500;
            var result = players.Values.Select(v => v.Sum()).Max();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private class Marble
        {
            public Marble() { Next = this; Prev = this; }
            public Marble Next;
            public Marble Prev;
            public long Value;
        }
    }
}
