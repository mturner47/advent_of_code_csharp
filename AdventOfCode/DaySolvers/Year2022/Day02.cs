namespace AdventOfCode.Year2022
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.Select(l => l.Split(" ")).Select(p => GetWinLoseDrawPoints(p[0], p[1]) + GetPlayedPoints(p[1])).Sum();
        }

        public object HardSolution(IList<string> lines)
        {
            var pointDictionary = new Dictionary<string, double>
            {
                { "A X", 0 + 3 },
                { "B X", 0 + 1 },
                { "C X", 0 + 2 },
                { "A Y", 3 + 1 },
                { "B Y", 3 + 2 },
                { "C Y", 3 + 3 },
                { "A Z", 6 + 2 },
                { "B Z", 6 + 3 },
                { "C Z", 6 + 1 },
            };

            return lines.Select(l => pointDictionary[l]).Sum();
        }

        private static int GetPlayedPoints(string playerChoice)
        {
            return playerChoice switch
            {
                "X" => 1,
                "Y" => 2,
                "Z" => 3,
                _ => 0,
            };
        }

        private static int GetWinLoseDrawPoints(string playerChoice, string opponentChoice)
        {
            return (playerChoice, opponentChoice) switch
            {
                ("A", "X") => 3,
                ("A", "Y") => 6,
                ("A", "Z") => 0,
                ("B", "X") => 0,
                ("B", "Y") => 3,
                ("B", "Z") => 6,
                ("C", "X") => 6,
                ("C", "Y") => 0,
                ("C", "Z") => 3,
                _ => 0,
            };
        }
    }
}
